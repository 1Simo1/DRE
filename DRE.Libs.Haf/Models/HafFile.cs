using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
