namespace DRE.Libs.SaveGame.Models
{
    public class SaveGameInfo
    {
        public String FileName { get; set; }
        public int Key { get; set; }

        public int PlayerIndex { get; set; }

        public int Level { get; set; }

        public bool UseWeapons { get; set; }
        public String SaveGameName { get; set; }

    }
}
