using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Uno.Extensions.Navigation;
using Uno.Extensions.Hosting;
using Uno.Extensions.Configuration;
using System;

namespace DRE.ViewModels
{
	public class ShellViewModel
	{
		private INavigator Navigator { get; }

	

			
		public ShellViewModel(INavigator navigator)
		
		{
			Navigator = navigator;


			Navigator.NavigateViewModelAsync<HomeViewModel>(this);

		}

		
	}
}
