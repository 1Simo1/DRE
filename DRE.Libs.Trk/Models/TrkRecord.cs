namespace DRE.Libs.Trk.Models
{
    public class TrkRecord
    {
        public int Id { get; set; }

        public int TrkId { get; set; }

        public String Car { get; set; }

        public String Name { get; set; }

        /// <summary>
        /// Lap Time, in hundredths of a second
        /// </summary>
        public int LapTime { get; set; }



    }
}
