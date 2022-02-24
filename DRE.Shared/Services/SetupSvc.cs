using Dapper;
using DRE.Interfaces;
using DRE.Libs.Lng.Models;
using DRE.Libs.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void SetupLanguage(String languageCode) => _lib.SetupLanguage(languageCode);

        public String SelectedLanguageCode() => _lib.SelectedLanguageCode();

    }
}
