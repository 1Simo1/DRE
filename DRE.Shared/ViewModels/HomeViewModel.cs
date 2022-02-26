using DRE.Interfaces;
using DRE.Libs.Lng;
using DRE.Models;
using System;
using System.Collections.ObjectModel;
using Uno.Extensions.Navigation;

namespace DRE.ViewModels
{
	public class HomeViewModel : VM
    {
        private ObservableCollection<NavItem> _nl;

        public ObservableCollection<NavItem> NavList
        {
            get { return _nl; }
            set { SetProperty(ref _nl, value); }
        }

        private NavItem _sn;
        public NavItem SelectedNav
        {
            get { return _sn; }
            set {
                   
                    if (value!=null && !value.NavPath.Equals(_sn?.NavPath)) {

                        Navigator.NavigateRouteAsync(this, $"/./Content/{value.NavPath}");
                    }

                SetProperty(ref _sn, value);
            }
        }

        public String ProjectName { get; set; }

        public String ProjectFolder { get; set; }

        public String DRE_Version { get; set; }

        public INavSvc NavSvc { get; }

        private INavigator Navigator { get; }

        public HomeViewModel(INavSvc navSvc, INavigator navigator)
        {
            NavSvc = navSvc;
            NavList = new ObservableCollection<NavItem>(NavSvc.SetNavLinks());

            ProjectName = navSvc.ProjectName();
            ProjectFolder = navSvc.ProjectFolder();
            DRE_Version = navSvc.DRE_Version();
            
            Navigator = navigator;



        }


    }
}
