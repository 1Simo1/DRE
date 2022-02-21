using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dre.Libs.Trk.Models
{
    public class TrkFile
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int trNumber { get; set; }

        public bool IsFlipped { get; set; }



    }
}
