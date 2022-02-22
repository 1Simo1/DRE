using DRE.Libs.Haf.Models;

namespace DRE.Libs.Haf
{
    public class LibHaf
    {
        private String dir { get; set; }
        public LibHaf(String gameFolderPath)
        {
            dir = gameFolderPath;
        }

        public List<HafFile> Init()
        {
            List<HafFile> haf = new List<HafFile>();

            byte[] file = System.IO.File.ReadAllBytes($"{dir}/ENDANI.haf");
            byte[] file_e = System.IO.File.ReadAllBytes($"{dir}/ENDANI0.HAF");
            byte[] file_s = System.IO.File.ReadAllBytes($"{dir}/SANIM.haf");

            haf.Add(new HafFile()
            {
                Id = 1,
                Name = $"ENDANI.haf",
                FrameNumber = 0,
                Data = file
            });

            haf.Add(new HafFile()
            {
                Id = 2,
                Name = $"ENDANI0.HAF",
                FrameNumber = 0,
                Data = file_e
            });

            haf.Add(new HafFile()
            {
                Id = 3,
                Name = $"SANIM.haf",
                FrameNumber = 0,
                Data = file_s
            });

            return haf;
        }
    }
}