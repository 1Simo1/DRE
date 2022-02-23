using DRE.Interfaces;
using DRE.Services;
using Uno.Extensions.Navigation;

namespace DRE.ViewModels
{
    public class ShellViewModel : VM
    {
        private INavigator Navigator { get; }
 
        public int Width { get; set; }
      
        public int Height { get; set; }
      


        public ShellViewModel(INavigator navigator, ISetupSvc setupSvc, ConfigSvc c)
        {

            Width = c.config.SavedWidth;

            Height = c.config.SavedHeight;

            Navigator = navigator;

            

            if (setupSvc.Setup)
            {
                Navigator.NavigateViewModelAsync<HomeViewModel>(this);
            }
            else Navigator.NavigateViewModelAsync<SetupDREViewModel>(this);

        }


    }
}
