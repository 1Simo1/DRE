using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Uno.Extensions.Navigation;
using Uno.Extensions.Hosting;
using Uno.Extensions.Configuration;
using System;
using DRE.Interfaces;

namespace DRE.ViewModels
{
	public class ShellViewModel : VM
	{
		private INavigator Navigator { get; }
			
		public ShellViewModel(INavigator navigator, ISetupSvc setupSvc)
		
		{
			Navigator = navigator;

			if (setupSvc.Setup)
			{
				Navigator.NavigateViewModelAsync<HomeViewModel>(this);
			} else Navigator.NavigateViewModelAsync<SetupDREViewModel>(this);

		}

		
	}
}
