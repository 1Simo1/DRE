using DRE.Libs.Cfg.Models;

namespace DRE.Libs.Cfg
{
    public class LibCfg
    {
        private String dir { get; set; }
        public LibCfg(String gameFolderPath)
        {
            dir = gameFolderPath;
        }

        /// <summary>
        /// Reads Hall of Fame from DR CFG file
        /// </summary>
        /// <returns>Hall of Fame detailed list</returns>
        public List<HofEntry> Hof()
        {
            List<HofEntry> hof = new List<HofEntry>();

            if (File.Exists($"{dir}/dr.cfg"))
            {
                byte[] file = System.IO.File.ReadAllBytes($"{dir}/dr.cfg");
                using (MemoryStream m = new MemoryStream(file))
                {
                    using (BinaryReader b = new BinaryReader(m))
                    {
                        b.ReadBytes(2678);
                        byte[] rd;
                        String nm;
                        int tr;
                        short lv;

                        for (short i = 1; i <= 10; i++)
                        {
                            rd = b.ReadBytes(12);
                            nm = "";
                            for (int n = 0; n < rd.Length; n++) { if (rd[n] != 0) nm += (char)rd[n]; }
                            tr = b.ReadInt32();
                            lv = (short)b.ReadInt32();

                            hof.Add(new HofEntry()
                            {
                                Id = i,
                                Name = nm,
                                TotalRaces = tr,
                                Level = lv
                            });
                        }
                    }
                }
            }



            return hof;
        }
    }
}