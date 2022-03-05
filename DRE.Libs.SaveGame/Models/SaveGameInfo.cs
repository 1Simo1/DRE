using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
