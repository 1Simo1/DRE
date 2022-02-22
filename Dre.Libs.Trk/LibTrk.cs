using DRE.Libs.Trk.Models;

namespace DRE.Libs.Trk
{
    public class LibTrk
    {
        private String dir { get; set; }    
        public LibTrk(String gameFolderPath)
        {
            dir = gameFolderPath;
        }

        public List<TrkFile> Init()
        {
            List<TrkFile> trk = new List<TrkFile>();

            trk.Add(new TrkFile() { Id = trk.Count+1, Name = "Suburbia", trNumber = 1, IsFlipped = false });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "Downtown", trNumber = 2, IsFlipped = false });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "Utopia", trNumber = 3, IsFlipped = false });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "Rock Zone", trNumber = 4, IsFlipped = false });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "Snake Alley", trNumber = 5, IsFlipped = false });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "Oasis", trNumber = 6, IsFlipped = false });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "Velodrome", trNumber = 7, IsFlipped = false });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "Holocaust", trNumber = 8, IsFlipped = false });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "Bogota", trNumber = 9, IsFlipped = false });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "West End", trNumber = 1, IsFlipped = true });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "Newark", trNumber = 2, IsFlipped = true });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "Complex", trNumber = 3, IsFlipped = true });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "Hell Mountain", trNumber = 4, IsFlipped = true });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "Desert Run", trNumber = 5, IsFlipped = true });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "Palm Side", trNumber = 6, IsFlipped = true });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "Eidolon", trNumber = 7, IsFlipped = true });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "Toxic Dump", trNumber = 8, IsFlipped = true });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "Borneo", trNumber = 9, IsFlipped = true });
            trk.Add(new TrkFile() { Id = trk.Count + 1, Name = "The Arena", trNumber = 0, IsFlipped = false });

            return trk;
        }

        public List<TrkRecord> defaultTrackRecords()
        {
            List<TrkRecord> crd = new List<TrkRecord>();

            String[] cars = new String[6] { "VAGABOND", "DERVISH", "SENTINEL", "SHRIEKER", "WRAITH", "DELIVERATOR" };

            if (File.Exists($"{dir}/dr.cfg"))
            {
                byte[] file = System.IO.File.ReadAllBytes($"{dir}/dr.cfg");
                using (MemoryStream m = new MemoryStream(file))
                {
                    using (BinaryReader b = new BinaryReader(m))
                    {
                        b.ReadBytes(86);
                        byte[] rd;
                        string nm;
                        Int16 t;
                        string c;
                        for (short i = 1; i <= 108; i++)
                        {
                            rd = b.ReadBytes(12);
                            nm = "";
                            for (int n = 0; n < rd.Length; n++) { if (rd[n] != 0) nm += (char)rd[n]; }
                            t = (Int16)(b.ReadInt32() * 6000 + b.ReadInt32() * 100 + b.ReadInt32());
                            c = cars[(int)(Math.Floor((decimal)(i - 1) / 18))];

                            crd.Add(new TrkRecord()
                            {
                                Id = i,
                                TrkId = (int)(Math.Floor((decimal)(i - 1) % 18)) + 1,
                                Car = c,
                                Name = nm,
                                LapTime = t
                            });


                        }
                    }
                }
            }


            return crd;

        }
    }
}