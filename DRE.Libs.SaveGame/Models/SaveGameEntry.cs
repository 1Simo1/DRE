namespace DRE.Libs.SaveGame.Models
{
    public class SaveGameEntry
    {
        public int id { get; set; }
        public String FileName { get; set; }

        public int Position { get; set; }

        public int AttributeNumber { get; set; }

        public int Value { get; set; }

        public String ValueText { get; set; }

        public override string ToString() => Position==0 ? $"{FileName} - {ValueText}" : ValueText;
        


    }
}
