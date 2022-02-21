using DRE.Libs.Bpa.Models;

namespace DRE.Libs.Bpa
{
    public class LibBpa
    {

        private String dir { get; set; }

        private List<BpaFile> bpa_list { get; set; }   
        
        /// <summary>
        /// Lists BPAs in game folder
        /// </summary>
        /// <param name="gameFolderPath">Game Path</param>
        /// <returns>Bpa File List with file name and size</returns>
        public List<BpaFile> List(String gameFolderPath)
        {

            dir = gameFolderPath;

            if (bpa_list == null) bpa_list = new List<BpaFile>();

            var bpaFiles = Directory.EnumerateFiles(gameFolderPath, "*.BPA");
            String nBpa = ""; /*/path/file.BPA => file */
            foreach (String nf in bpaFiles)
            {

                nBpa = nf.Substring(gameFolderPath.Length + 1);

                bpa_list.Add(new BpaFile()
                {
                    Name = nBpa.Substring(0, nBpa.Length - 4),
                    Size = new FileInfo(nf).Length
                });
                    
            }

            return bpa_list;
        }

        /// <summary>
        /// Reads and computes BPA file from game folder
        /// </summary>
        /// <param name="bpaName">BPA file name</param>
        /// <returns>List of BPA archive entries, with data details</returns>

        public List<BpaFileEntry> BpaFileEntries(String bpaName)
        {
            List<BpaFileEntry> bpaFileEntries = new List<BpaFileEntry>();

            var nf = bpaName.Contains("BPA") ? bpaName : $"{bpaName}.BPA";

           
            using (MemoryStream m = new MemoryStream(File.ReadAllBytes(nf)))
            {
                using (BinaryReader b = new BinaryReader(m))
                {
                    var bpaHeader = b.ReadBytes(4 + 255 * 17);

                    var h = new BinaryReader(new MemoryStream(bpaHeader));

                    int n = h.ReadInt32(); //Total file number in BPA

                    for (int i=1;i<=n;i++)
                    {
                        Byte[] bfn = h.ReadBytes(13); //Bytes for entry file name
                        String FileEntryName = String.Empty;

                        for (int f = 0; f <= 12; f++)
                        {
                            if (bfn[f] == 0) break;
                            char c = (char)(bfn[f] - (117 - 3 * f));
                            FileEntryName += c;
                        }

                        int entryDataSize = h.ReadInt32();

                        bpaFileEntries.Add(new BpaFileEntry() { 
                        
                            FileName = FileEntryName,
                            Size = entryDataSize,
                            Data = b.ReadBytes(entryDataSize)
                        
                        });

                    }

                } 
            
            }

            return bpaFileEntries;
        }
    }
}