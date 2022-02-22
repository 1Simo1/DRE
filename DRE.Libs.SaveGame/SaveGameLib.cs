using DRE.Libs.SaveGame.Models;

namespace DRE.Libs.SaveGame
{
    public class SaveGameLib
    {
        private String dir { get; set; }

        public SaveGameLib(String gameFolderPath)
        {
            dir = gameFolderPath;
        }

        public List<SaveGameEntry> Init()
        {

            List<SaveGameEntry> sgd = new List<SaveGameEntry>();

            var sgFiles = Directory.EnumerateFiles(dir, "DR.SG*");
            byte[] file;
            int k, r, id, w, d, x, i;
            String sgn, nmp, nsg;
            byte[] temp;
          
            foreach (String sg in sgFiles)
            {
                nsg = sg.Substring(dir.Length + 1);
                file = System.IO.File.ReadAllBytes(sg);
                k = file[0];

                sgd.Add(new SaveGameEntry()
                {
                    FileName = nsg,
                    Position = 0,
                    AttributeNumber = 0,
                    Value = k,
                    ValueText = String.Empty
                });

                for (i = 1; i <= 2178; i++)
                {
                    x = file[i];
                    r = i % 6;
                    x = (x << r | x >> (8 - r)) - (17 * i) + k;
                    x %= 256;
                    if (x < 0) x += 256;

                    file[i] = (byte)x;
                }
                id = file[1];
                w = file[2];
                d = file[3];

                sgd.Add(new SaveGameEntry()
                {
                    FileName = nsg,
                    Position = 0,
                    AttributeNumber = 1,
                    Value = id,
                    ValueText = String.Empty
                });

                sgd.Add(new SaveGameEntry()
                {
                    FileName = nsg,
                    Position = 0,
                    AttributeNumber = 2,
                    Value = w,
                    ValueText = String.Empty
                });

                sgd.Add(new SaveGameEntry()
                {
                    FileName = nsg,
                    Position = 0,
                    AttributeNumber = 3,
                    Value = d,
                    ValueText = String.Empty
                });

                using (MemoryStream m = new MemoryStream(file))
                {
                    using (BinaryReader b = new BinaryReader(m))
                    {
                        b.ReadInt32(); //k id w d
                        temp = b.ReadBytes(15);
                        sgn = ""; //SaveGame Name (in game)
                        for (i = 0; i < temp.Length; i++)
                        {
                            if (temp[i] != 0) sgn += (char)temp[i];
                        }

                        sgd.Add(new SaveGameEntry()
                        {
                            FileName = nsg,
                            Position = 0,
                            AttributeNumber = 4,
                            Value = 0,
                            ValueText = sgn
                        });

                        for (int p = 1; p <= 20; p++)
                        {
                            temp = b.ReadBytes(12);
                            nmp = "";// Driver Name
                            for (i = 0; i < temp.Length; i++)
                            {
                                if (temp[i] != 0) nmp += (char)temp[i];
                            }

                            sgd.Add(new SaveGameEntry()
                            {
                                FileName = nsg,
                                Position = p,
                                AttributeNumber = 0,
                                Value = 0,
                                ValueText = nmp
                            });

                            for (i = 1; i <= 24; i++)
                            {
                                sgd.Add(new SaveGameEntry()
                                {
                                    FileName = nsg,
                                    Position = p,
                                    AttributeNumber = i,
                                    Value = b.ReadInt32(),
                                    ValueText = String.Empty
                                });

                            }
                        }
                    }
                }

            }
        
        
            return sgd;
        }
    }
}