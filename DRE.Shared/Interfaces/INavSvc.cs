using DRE.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DRE.Interfaces
{
    public interface INavSvc
    {
        /// <summary>
        /// Default selected language code
        /// </summary>
        String L { get; set; }  

        /// <summary>
        /// Method to setup/init main navigation link list
        /// </summary>
        /// <returns>Navigation item list for DRE</returns>
        List<NavItem> SetNavLinks();

        String ProjectName();
        String ProjectFolder();
        String DRE_Version();


    }
}
