namespace DRE.Libs.Haf.Models
{
    public class HafFile
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int FrameNumber { get; set; }
        public Byte[] Data { get; set; }
    }
}
