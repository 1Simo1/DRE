using Dapper;
using DRE.Interfaces;
using DRE.Libs.Lng;
using DRE.Libs.Setup;
using DRE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRE.Services
{
    public class NavSvc : INavSvc
    {
        private readonly LibLng _libLng;

        public String L { get; set; }

        public NavSvc()
        {
            _libLng = new LibLng();
            L = LibSetup.db.Query<String>("SELECT v FROM dre WHERE n='defaultLanguage'").First();
        }

        /**
         * "Static implementation" 
         * 
         */
        public List<NavItem> SetNavLinks()
        {
            var NavList = new List<NavItem>();

            NavList.Add(new NavItem()
            {
                NavPath = "BPA",
                Icon = "\uE7B8",
                Text = _libLng._("opt_bpa",L)
            });

            NavList.Add(new NavItem()
            {
                NavPath = "SG",
                Icon = "\uE792",
                Text = _libLng._("opt_sg",L)
            });

            NavList.Add(new NavItem()
            {
                NavPath = "TRK",
                Icon = "\uEDC6",
                Text = _libLng._("opt_trk",L)
            });

            return NavList;
        }

        public String ProjectName() => LibSetup.db.Query<String>("SELECT v FROM dre WHERE n='p_dre'").First();
       
        public String ProjectFolder() => LibSetup.db.Query<String>("SELECT v FROM dre WHERE n='c_dre'").First();
       
        public String DRE_Version() => LibSetup.db.Query<String>("SELECT v FROM dre WHERE n='dre_v'").First();

    }
}
