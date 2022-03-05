using Dapper;
using DRE.Libs.Bpa.Models;
using DRE.Libs.Setup.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;

namespace DRE.Services
{
    public class SvcBPA
    {
        private IDbConnection db { get; }

        private String dir { get; } = $"{AppDomain.CurrentDomain.BaseDirectory}files";

        private String gameFolder { get => db.Query<String>("SELECT v FROM DRE WHERE n='c_dre'").First(); }

        private IProgress<SetupProgress> x { get; set; }

        public SvcBPA(IDbConnection DRE_db) => db = DRE_db;

        private void testDir(String path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public List<BpaFile> ListBpa() => db.Query<BpaFile>("SELECT id,nf AS Name, dim AS Size FROM bpa ORDER BY id ASC").ToList();

        public List<BpaFileEntry> BpaFileList(int id)
        {
            return db.Query<BpaFileEntry>("SELECT id, nf AS FileName, bpa AS bpaID, n, dim AS Size, pal,exp, d as Data " +
                                          "FROM bpa_files WHERE bpa=@bpa_id ORDER BY n ASC", new { bpa_id = id }).ToList();
        }

        public List<String> computeBpaFileEntryAvailaibleOperations(BpaFileEntry value)
        {
            List<String> opList = new List<String>();

            if (value.FileName.EndsWith("BPK"))
            {
                opList.Add("ext_bpk");
                opList.Add("ext_img");
            }

            if (value.FileName.Equals("CHATTEXT.BPK") || value.FileName.Contains("SCE.BPK") || value.FileName.Contains("SHA.BPK"))
            {
                opList.Remove("ext_img");
            }


            opList.Add("ext_file");
            opList.Add("agg");


            return opList;
        }

        public void bpaFileEntryOperation(BpaFileEntry bpaFile, String opCode, IProgress<SetupProgress> xp)
        {
            x = xp;


            x.Report(new SetupProgress() { p = 1 });

            switch (opCode)
            {
                case "ext_bpk": bpkExp(bpaFile); break;
                case "ext_img": bpkImg(bpaFile); break;
                case "ext_file": extractFileFromBPA(bpaFile); break;
                case "agg": updateFileIntoBPA(bpaFile); break;
                default: break;
            }

            x.Report(new SetupProgress() { p = 100 });
        }

        /// <summary>
        /// BPK decoder/expander method
        /// </summary>
        /// <param name="bpaFile">BPK file from BPA to expand</param>
        /// <param name="x">Operation progress provider</param>
        private void bpkExp(BpaFileEntry bpaFile)
        {

            String bpaName = db.Query<String>("SELECT nf FROM bpa WHERE id=@bpa_id", new { bpa_id = bpaFile.bpaID }).First();

            testDir($"{dir}");
            testDir($"{dir}/BPA");
            testDir($"{dir}/BPA/{bpaName}");

            String file_path = $"{dir}/BPA/{bpaName}/{bpaFile.FileName}.EXP";

            Byte[] exp;

            if (bpaFile.exp == 0)
            { // EXP Data not yet in DB

                MemoryStream md = new MemoryStream(bpaFile.Data);

                List<int> codList = ReadBpkCodes(md);

                /// Byte list to write EXP file
                List<Byte> w = new List<byte>();

                if (codList.LastIndexOf(257) == 0)
                {
                    w.AddRange(Expand_Bpk_Block(codList));
                }
                else
                {
                    var b = IndexesOf(codList, 257);

                    for (int i = 0; i < b.Count - 1; i++)
                    {
                        w.AddRange(Expand_Bpk_Block(codList.GetRange(b[i], (b[i + 1]) - b[i])));
                        x.Report(new() { p = (10 * i / b.Count) + 41 });
                    }

                    w.AddRange(Expand_Bpk_Block(codList.GetRange(b[b.Count - 1], codList.Count - b[b.Count - 1])));
                }


                exp = w.ToArray();

                int exp_new_id = bpaFile.id;

                ExpFileEntry expNewFile = setExpFile(bpaFile.FileName, exp, exp_new_id);

                db.Query("REPLACE INTO exp VALUES (@id,@d,@w,@h,@rix)", new
                {
                    id = bpaFile.id,
                    d = exp,
                    w = expNewFile.width,
                    h = expNewFile.height,
                    rix = expNewFile.rix
                });


                db.Query("UPDATE bpa_files SET exp=@id WHERE id=@id", new { id = bpaFile.id });

                bpaFile.exp = bpaFile.id;



            }
            else
            { // EXP data already in DB
                exp = db.Query<ExpFileEntry>("SELECT d as Data FROM exp WHERE id=@exp_id", new { exp_id = bpaFile.exp }).First().Data;
            }

            x.Report(new() { p = 51 });

            File.WriteAllBytes(file_path, exp);

        }

        private List<Byte> Expand_Bpk_Block(List<int> blockCodes)
        {
            List<Byte> w = new List<byte>(); //Resulting byte list for bpk block

            int l = blockCodes.Count;

            // Limits block at last "End of Information Code" occurence
            for (int x = blockCodes.Count - 1; x >= 0; x--)
            {
                if (blockCodes[x] == 256)
                {
                    l = x;
                    break;
                }
            }

            Dictionary<int, String> d = new Dictionary<int, string>(); //Dictionary for decompression
            int c, cc, p;
            for (int r = 0; r <= 255; r++)
            {
                c = (ushort)((r << 5 | r >> 3) % 256); //Palette values in BPK are shuffled as compared to those in game
                d.Add(r, ((char)c).ToString());
            }
            d.Add(256, ""); //EOI (End of Information and Clear Code are swapped as compared to GIF algorithm)
            d.Add(257, ""); //CC (Clear Code, see note above)

            //Algorithm block
            String k = "";

            cc = blockCodes[0];


            if (blockCodes.Count > 1 && blockCodes[1] != 256 && d.ContainsKey(blockCodes[1]))
            {
                w.Add((byte)d[blockCodes[1]][0]);
            }


            for (int i = 1; i < l - 1; i++)
            {

                p = blockCodes[i];
                c = blockCodes[i + 1];

                if (d.ContainsKey(c) && d[c].Length != 0)
                {

                    for (int x = 0; x < d[c].Length; x++) w.Add((byte)d[c][x]);

                    k = "" + d[c][0];
                    d.Add(d.Count, d[p] + k);

                }
                else
                {
                    if (d[p].Length != 0)
                    {

                        k = "" + d[p][0];

                        for (int x = 0; x < d[p].Length; x++) w.Add((byte)d[p][x]);
                        for (int x = 0; x < k.Length; x++) w.Add((byte)k[x]);
                    }
                    d.Add(d.Count, d[p] + k);
                }
            }

            return w;
        }

        private List<int> IndexesOf(List<int> codList, int v)
        {
            List<int> indexes = new List<int>();

            for (int i = 0; i < codList.Count; i++)
            {
                if (codList[i].Equals(v)) indexes.Add(i);
            }

            return indexes;
        }

        private List<int> ReadBpkCodes(MemoryStream md)
        {
            List<int> cod = new List<int>();
            int nbits = 9;
            int tbits = 0;
            String temp = "";
            String cs = "";
            UInt16 c = 0;
            int xb = 0;

            using (BinaryReader bpk = new BinaryReader(md))
            {
                for (int i = 0; i < md.Length; i++)
                {
                    temp = Convert.ToString(bpk.ReadByte(), 2).PadLeft(8, '0') + temp;
                    if (temp.Length >= nbits)
                    {

                        cs = temp.Substring(temp.Length - nbits);
                        c = Convert.ToUInt16(cs, 2);
                        cod.Add(c);

                        x.Report(new() { p = 1 + (40 * i / md.Length) });


                        if (c == 256) return cod;
                        tbits += nbits;

                        xb++;

                        temp = temp.Substring(0, temp.Length - nbits);
                        if (xb == (UInt16)Math.Pow(2, nbits - 1) && nbits < 12) { nbits++; xb = 0; }
                        if (c == 257 && nbits != 9)
                        {
                            nbits = 9;
                            xb = 1;
                        }
                    }
                }
            }

            return cod;
        }

        /// <summary>
        /// Method to compute EXP File info (computes width, height and rix values)
        /// </summary>
        /// <param name="fileName">BPK starting file name</param>
        /// <param name="exp">BPK decompressed data</param>
        /// <returns></returns>
        private ExpFileEntry setExpFile(String BPK_File_Name, Byte[] exp, int exp_id)
        {
            ExpFileEntry NewExp = new();

            NewExp.id = exp_id;

            int w = 0, h = 0;

            Dictionary<String, int> wh = new Dictionary<String, int>();

            var pixels = exp.Length;

            NewExp.rix = false;

            if (exp[0] == 82 && exp[1] == 73 && exp[2] == 88)
            { //RIX
                w = (int)(exp[4] + exp[5] * 256);
                h = (int)(exp[6] + exp[7] * 256);
                NewExp.rix = true;
            }
            else
            {
                switch (pixels)
                {
                    case 48: w = 8; h = 6; break;
                    case 192: w = 8; h = 24; break;
                    case 240: w = 10; h = 24; break;
                    case 256: w = 8; h = 32; break;
                    case 272: w = 136; h = 2; break;
                    case 320: w = 8; h = 40; break;
                    case 558: w = 31; h = 18; break;
                    case 576: w = 64; h = 9; break;
                    case 616: w = 22; h = 28; break;
                    case 756: w = 42; h = 18; break;
                    case 880: w = 8; h = 110; break;
                    case 954: w = 53; h = 18; break;
                    case 1152: w = 8; h = 144; break;
                    case 1200: w = 20; h = 60; break;
                    case 1536: w = 16; h = 96; break;
                    case 1818: w = 101; h = 18; break;
                    case 2048: w = 16; h = 128; break;
                    case 2250: w = 15; h = 150; break;
                    case 2288: w = 16; h = 143; break;
                    case 2304: w = 24; h = 96; break;
                    case 2560: w = 32; h = 80; break;
                    case 3072: w = 8; h = 384; break;
                    case >= 4096 and < 4128: w = 64; h = 64; break;
                    case 4128: w = 172; h = 24; break;
                    case 4352: w = 68; h = 64; break;
                    case 4380: w = 146; h = 30; break;
                    case 4704: w = 98; h = 48; break;
                    case 5376: w = 16; h = 336; break;
                    case 5440: w = 272; h = 20; break;
                    case 6144: w = 96; h = 64; break;
                    case 6400: w = 640; h = 10; break;
                    case 6696: w = 9; h = 744; break;
                    case 6936: w = 68; h = 102; break;
                    case 7040: w = 440; h = 16; break;
                    case 9216: w = 96; h = 96; break;
                    case 10240: w = 640; h = 16; break;
                    case 11776: w = 92; h = 128; break;
                    case 11960: w = 115; h = 104; break;
                    case 12672: w = 24; h = 528; break;
                    case 13312: w = 104; h = 128; break;
                    case 14400: w = 80; h = 180; break;
                    case 14948: w = 202; h = 74; break;
                    case 15504: w = 204; h = 76; break;
                    case 16384: w = 64; h = 256; break;
                    case 20000: w = 20; h = 1000; break;
                    case 20480: w = 640; h = 32; break;
                    case 20844: w = 54; h = 386; break;
                    case 21504: w = 96; h = 224; break;
                    case 22528: w = 32; h = 704; break;
                    case 24576: w = 16; h = 1536; break;
                    case 32256: w = 64; h = 504; break;
                    case 34560: w = 640; h = 54; break;
                    case 36000: w = 240; h = 150; break;
                    case 36480: w = 320; h = 114; break;
                    case 36864: w = 96; h = 384; break;
                    case 38400: w = 150; h = 256; break;
                    case 39744: w = 216; h = 184; break;
                    case 40000: w = 200; h = 200; break;
                    case 43520: w = 640; h = 68; break;
                    case 43940: w = 260; h = 169; break;
                    case 45056: w = 352; h = 128; break;
                    case 45466: w = 254; h = 179; break;
                    case 46080: w = 96; h = 480; break;
                    case 47800: w = 200; h = 239; break;
                    case 58368: w = 256; h = 228; break;
                    case 61440: w = 64; h = 960; break;
                    case 61650: w = 225; h = 274; break;
                    case 64000: w = 320; h = 200; break;
                    case 64768: w = 320; h = 200; break;
                    case 73728: w = 96; h = 768; break;
                    case 98304: w = 32; h = 3072; break;
                    case 98640: w = 360; h = 274; break;
                    case 104992: w = 272; h = 386; break;
                    case 110592: w = 96; h = 1152; break;
                    case 124800: w = 100; h = 1248; break;
                    case 131970: w = 530; h = 249; break;
                    case 136431: w = 489; h = 279; break;
                    case 153600: w = 40; h = 3840; break;
                    case 286720: w = 128; h = 2240; break;
                    case 307200: w = 640; h = 480; break;
                    default: break;
                }

                switch (BPK_File_Name)
                {
                    case "RASTI1.BPK": w = 64; h = 32; break;
                    case "SMALFO4A.BPK": w = 6; h = 384; break;
                    case "ROCKET1.BPK": w = 16; h = 384; break;
                    case "ROCKET2.BPK": w = 16; h = 384; break;
                    case "PEDESTR.BPK": w = 16; h = 576; break;
                    case "GEN-FLA.BPK": w = 80; h = 3840; break;
                    default: break;
                }

            }

            NewExp.Data = exp;

            NewExp.width = w;
            NewExp.height = h;

            return NewExp;
        }

        /// <summary>
        /// Writes image on disk starting from BPK
        /// </summary>
        /// <param name="bpaFile">BPK file from BPA</param>
        /// <param name="imageFormat">Optional specific output image format, if null all supported image formats are written</param>
        private void bpkImg(BpaFileEntry bpaFile, String imageFormat = null)
        {
            if (bpaFile.exp == 0)
            {
                bpkExp(bpaFile);
                bpaFile = db.Query<BpaFileEntry>(
                          "SELECT id,nf AS FileName, bpa AS bpaID, n, dim AS Size, pal, exp, d AS Data " +
                          "FROM bpa_files WHERE id=@id", new { id = bpaFile.id }).Single();
            }

            x.Report(new() { p = 51 });

            ExpFileEntry exp = db.Query<ExpFileEntry>(
                               "SELECT id, d AS Data, w AS width, h AS height, rix FROM exp WHERE id=@id", new { id = bpaFile.exp }).Single();

            PalFileEntry pal = bpaFile.pal == 0 ?
                               assignPAL(bpaFile) :
                               db.Query<PalFileEntry>("SELECT id, d AS Data FROM pal WHERE id=@id", new { id = bpaFile.pal }).Single();


            String bpaName = db.Query<String>("SELECT nf FROM bpa WHERE id=@id", new { id = bpaFile.bpaID }).First();

            testDir($"{dir}");
            testDir($"{dir}/BPA");
            testDir($"{dir}/BPA/{bpaName}");

            String baseFileName = $"{dir}/BPA/{bpaName}/{bpaFile.FileName}";

            if (String.IsNullOrEmpty(imageFormat))
            {
                imgRIX(exp, pal, baseFileName);
                x.Report(new() { p = 55 });
                imgGIF(exp, pal, baseFileName);
                x.Report(new() { p = 75 });
                imgBMP(exp, pal, baseFileName);
                x.Report(new() { p = 95 });
            }
            else
            {
                switch (imageFormat)
                {
                    case "RIX": imgRIX(exp, pal, baseFileName); break;
                    case "GIF": imgGIF(exp, pal, baseFileName); break;
                    case "BMP": imgBMP(exp, pal, baseFileName); break;
                    default: return;
                }
            }
            x.Report(new() { p = 95 });
        }

        private void imgRIX(ExpFileEntry exp, PalFileEntry pal, String baseFileName)
        {
            if (exp.rix)
            {
                File.WriteAllBytes($"{baseFileName}.RIX", exp.Data);
                return;
            }

            List<Byte> rix = new List<Byte>();

            rix.AddRange(new byte[] { 82, 73, 88, 51 }); //RIX3 

            rix.Add((byte)(exp.width % 256)); //w LSB
            rix.Add((byte)(exp.width / 256)); //w MSB

            rix.Add((byte)(exp.height % 256)); //h LSB
            rix.Add((byte)(exp.height / 256)); //h MSB

            rix.AddRange(new byte[] { 175, 0 }); //VGA & 0

            rix.AddRange(pal.Data);

            rix.AddRange(exp.Data);

            File.WriteAllBytes($"{baseFileName}.RIX", rix.ToArray());
        }

        private void imgGIF(ExpFileEntry exp, PalFileEntry pal, String baseFileName)
        {

            if (exp.rix)
            {
                exp.Data = exp.Data.ToList().GetRange(778, exp.Data.Length - 778).ToArray();
                exp.rix = false; //Prevents double header removal in memory due to possible multiple image formats writing
            }
            using (var image = new Bitmap(exp.width, exp.height))
            {
                int np = 0;
                for (int w = 0; w < exp.height; w++)
                {
                    for (int h = 0; h < exp.width; h++)
                    {
                        image.SetPixel(h, w, Color.FromArgb(
                            pal.Data[(exp.Data[np]) * 3] * 4,
                            pal.Data[(exp.Data[np]) * 3 + 1] * 4,
                            pal.Data[(exp.Data[np]) * 3 + 2] * 4
                            ));

                        np++;
                    }
                }

                image.Save($"{baseFileName}.GIF", System.Drawing.Imaging.ImageFormat.Gif);
            }
        }
        private void imgBMP(ExpFileEntry exp, PalFileEntry pal, String baseFileName)
        {
            if (exp.rix)
            {
                exp.Data = exp.Data.ToList().GetRange(778, exp.Data.Length - 778).ToArray();
                exp.rix = false; //Prevents double header removal in memory due to possible multiple image formats writing
            }

            using (var image = new Bitmap(exp.width, exp.height))
            {
                int np = 0;
                for (int w = 0; w < exp.height; w++)
                {
                    for (int h = 0; h < exp.width; h++)
                    {
                        image.SetPixel(h, w, Color.FromArgb(
                           (pal.Data[(exp.Data[np] * 3)] * 4),
                            (pal.Data[(exp.Data[np] * 3) + 1] * 4),
                            (pal.Data[(exp.Data[np] * 3) + 2] * 4)

                            ));

                        np++;
                    }
                }

                image.Save($"{baseFileName}.BMP", System.Drawing.Imaging.ImageFormat.Bmp);
            }
        }


        /// <summary>
        /// Method for associate right palette to image from BPK
        /// </summary>
        /// <param name="bpaFile">BPK file into BPA</param>
        /// <returns>Palette id and data</returns>
        private PalFileEntry assignPAL(BpaFileEntry bpaFile)
        {
            //Test RIX : if true, palette is in exp data header

            var exp = db.Query<ExpFileEntry>(
               "SELECT id, d AS Data, w AS width, h AS height, rix FROM exp WHERE id=@id", new { id = bpaFile.exp }).FirstOrDefault();

            if (exp.rix)
            {
                return new PalFileEntry()
                {
                    id = 1000 + exp.id,
                    Data = exp.Data.ToList().GetRange(10, 768).ToArray()
                };
            }

            // PAL already in DB
            if (bpaFile.pal != 0) return db.Query<PalFileEntry>("SELECT id, d AS Data FROM pal WHERE id=@id", new { id = bpaFile.pal }).Single();

            List<BpaFileEntry> filesBPA = BpaFileList(bpaFile.bpaID);


            /* Search for PAL file with BPK file of same name */
            var testPAL = filesBPA.Where
                (x => x.FileName.Equals($"{bpaFile.FileName.Substring(0, bpaFile.FileName.Length - 3)}PAL")).ToList();

            if (testPAL.Any())
            {
                db.Query("REPLACE INTO pal VALUES (@id,@d)", new { id = testPAL.First().id, d = testPAL.First().Data });
                db.Query("UPDATE bpa_files SET pal=@p WHERE id=@id", new { p = testPAL.First().id, id = bpaFile.id });

                return new PalFileEntry()
                {
                    id = testPAL.First().id,
                    Data = testPAL.First().Data
                };
            }

            /* Default : Assign MENU.PAL palette to image */
            testPAL = db.Query<BpaFileEntry>("SELECT id, nf AS FileName, bpa AS bpaID, n, dim AS Size, pal,exp, d as Data " +
                                             "FROM bpa_files WHERE nf=@m ORDER BY n ASC", new { m = "MENU.PAL" }).ToList();

            return new PalFileEntry()
            {
                id = testPAL.First().id,
                Data = testPAL.First().Data
            };



        }

        /// <summary>
        /// Extracts file from BPA, as is in archive
        /// </summary>
        /// <param name="bpaFile">File into BPA to extract</param>
        private void extractFileFromBPA(BpaFileEntry bpaFile)
        {
            String bpaName = db.Query<String>("SELECT nf FROM bpa WHERE id=@bpa_id", new { bpa_id = bpaFile.bpaID }).First();

            testDir($"{dir}");
            testDir($"{dir}/BPA");
            testDir($"{dir}/BPA/{bpaName}");

            String file_path = $"{dir}/BPA/{bpaName}/{bpaFile.FileName}";

            File.WriteAllBytes(file_path, bpaFile.Data);
        }


        /// <summary>
        /// Updates file into DRE DB BPA informations, not on game folder
        /// </summary>
        /// <param name="bpaFile">BPA file entry to update on Editor DB</param>
        private void updateFileIntoBPA(BpaFileEntry bpaFile)
        {
            String bpaName = db.Query<String>("SELECT nf FROM bpa WHERE id=@bpa_id", new { bpa_id = bpaFile.bpaID }).First();

            testDir($"{dir}");
            testDir($"{dir}/BPA");
            testDir($"{dir}/BPA/{bpaName}");

            String file_path = $"{dir}/BPA/{bpaName}/{bpaFile.FileName}";

            if (File.Exists(file_path))
            {

                bpaFile.Data = File.ReadAllBytes(file_path);

                db.Query("UPDATE bpa_files SET d=@fd WHERE id=@id", new
                {
                    fd = bpaFile.Data,
                    id = bpaFile.id
                });

                if (bpaFile.exp != 0)
                {
                    db.Query("DELETE FROM exp WHERE id=@id", new { id = bpaFile.exp });
                    db.Query("UPDATE bpa_files SET exp=0 WHERE id=@id", new { id = bpaFile.id });
                }

            }

        }

        public void ExtractImagesFromBPAs(IProgress<SetupProgress> xp)
        {
            x = xp;

            x.Report(new SetupProgress() { p = 1 });

            foreach (var bpaFile in ListBpa())
            {
                foreach (var bpaFileEntry in BpaFileList(bpaFile.id))
                {
                    if (computeBpaFileEntryAvailaibleOperations(bpaFileEntry).Contains("ext_img"))
                    {

                        bpkImg(bpaFileEntry);
                    }
                }
            }

            x.Report(new SetupProgress() { p = 100 });
        }


        /// <summary>
        /// OVERWRITES selected BPA file in game folder
        /// </summary>
        /// <param name="selectedBPA">Selected BPA</param>
        public void WriteBPA(BpaFile selectedBPA, IProgress<SetupProgress> xp)
        {
            if (selectedBPA == null) return;

            x = xp;

            List<BpaFileEntry> fileList = BpaFileList(selectedBPA.id);

            List<Byte> header = new List<Byte>();

            List<Byte> data = new List<Byte>();

            header.AddRange(new Byte[4] { (byte)fileList.Count, 0, 0, 0 });


            int n = 0;
            foreach (var bpaFileEntry in fileList)
            {
                x.Report(new SetupProgress() { p = 100 * n / fileList.Count });

                for (int i = 0; i < bpaFileEntry.FileName.Length; i++)
                {
                    int c = (byte)bpaFileEntry.FileName[i] + 117 - 3 * i;
                    header.Add((byte)c);
                }

                for (int i = bpaFileEntry.FileName.Length; i < 13; i++) header.Add(0);

                header.AddRange(BitConverter.GetBytes(bpaFileEntry.Size));

                data.AddRange(bpaFileEntry.Data);
                n++;
            }

            header.AddRange(new Byte[(255 - fileList.Count) * 17]);


            List<Byte> bpa = new();

            bpa.AddRange(header);
            bpa.AddRange(data);

            ///OVERWRITES BPA FILE IN GAME FOLDER!
            File.WriteAllBytes($"{gameFolder}/{selectedBPA.Name}.BPA", bpa.ToArray());

            x.Report(new SetupProgress() { p = 100 });
        }




    }
}
