using DRE.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Uno.Extensions.Navigation;

namespace DRE.ViewModels
{
    public class ShellViewModel : VM
    {

        private INavigator Navigator { get; }
        public ShellViewModel(ISetupSvc setupSvc, INavigator navigator)
        {
            Navigator = navigator;

            if (setupSvc.Setup)
            {
                Navigator.NavigateViewModelAsync<HomeViewModel>(this);
            } else Navigator.NavigateViewModelAsync<SetupLngViewModel>(this);

        }
    }
}
