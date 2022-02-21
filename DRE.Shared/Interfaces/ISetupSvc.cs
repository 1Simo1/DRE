using System;
using System.Collections.Generic;
using System.Text;

namespace DRE.Interfaces
{
    public interface ISetupSvc
    {
        /// <summary>
        /// Checks Setup state : if true, at least one project ready, else initial setup needed
        /// </summary>
        public bool Setup { get; set; }
    }
}
