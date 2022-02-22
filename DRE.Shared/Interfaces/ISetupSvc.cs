using DRE.Libs.Lng.Models;
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

        /// <summary>
        /// Loads localizations from "locale" folder
        /// </summary>
        /// <returns>List of available languages for editor</returns>
        public List<Localization> LoadLngList();
    }
}
