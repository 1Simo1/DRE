using Dre.Libs.Trk.Models;

namespace Dre.Libs.Trk
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
    
        
    
    }
}