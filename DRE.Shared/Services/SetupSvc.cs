using DRE.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DRE.Services
{
    public class SetupSvc : ISetupSvc
    {

        private bool _s;

        public bool Setup
        {
            get { return _s; }
            set { _s = value; }
        }

  

        public SetupSvc()
        {

        }

    }
}
