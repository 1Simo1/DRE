using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRE.Libs.Bpa.Models
{
    public class BpaFileEntry
    {
        public String FileName { get; set; }
        public int Size { get; set; }

        public Byte[] Data { get; set; }
    }
}
