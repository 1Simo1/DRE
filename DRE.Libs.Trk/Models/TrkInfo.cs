using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRE.Libs.Trk.Models
{
    public class TrkInfo
    {
        public int fullImageWidth { get; set; }
        public int fullImageHeight { get; set;}

        public int totalZones { get; set; }

        public List<TrkDriverStartInfo> driverStartPositions { get; set; }

        public List<TrkPowerUpInfo> powerUpPositions { get; set; }

        public List<TrkPedestrianInfo> pedestrianPositions { get; set; }


    }
}
