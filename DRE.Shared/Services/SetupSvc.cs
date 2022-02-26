using DRE.Interfaces;
using DRE.Libs.Lng.Models;
using DRE.Libs.Setup;
using DRE.Libs.Setup.Models;
using System;
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

        public void SetupLanguage(String languageCode) => _lib.SetupLanguage(languageCode);

        public String SelectedLanguageCode() => _lib.SelectedLanguageCode();

        public void SetupNewProject(String p_dre, String c_dre, IProgress<SetupProgress> x) => _lib.Setup(p_dre, c_dre, x);

    }
}
