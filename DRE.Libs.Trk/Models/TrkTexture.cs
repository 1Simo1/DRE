namespace DRE.Libs.Trk.Models
{
    public class TrkTexture
    {

        public int width { get; set; }
        public int height { get; set; }

        public int offset { get; set; }

        public int mapX { get; set; }

        public int mapY { get; set; }

        public int size_multiplier { get; set; }

        public int realX { get; set; }

        public int realY { get; set; }

        public int Unknown_Unused { get; set; }

        public int Linked3DObj { get; set; }

        public int Linked3DObjPolygon { get; set; }

        public Byte[] Data { get; set; }

    }
}
