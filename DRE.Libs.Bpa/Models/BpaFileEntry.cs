using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRE.Libs.Bpa.Models
{
    public class BpaFileEntry
    {
        public int id { get; set; }
        public String FileName { get; set; }
        public int bpaID { get; set; }
        public int n { get; set; }
        public int Size { get; set; }
        public int pal { get; set; }
        public int exp { get; set; }
        public Byte[] Data { get; set; }

        public override string ToString() => FileName;
       
    }
}
