using DRE.Interfaces;
using DRE.Libs.Lng.Models;
using DRE.Libs.Setup;
using System.Collections.Generic;

namespace DRE.Services
{
    public class SetupSvc : ISetupSvc
    {

        private bool _s;
        private readonly LibSetup _lib;

        public bool Setup
        {
            get { return _s; }
            set { _s = value; }
        }

        public SetupSvc()
        {
            _lib = new LibSetup();

            Setup = _lib.testSetup();
           
        }

        public List<Localization> LoadLngList() => _lib.LngList();
       
    }
}
