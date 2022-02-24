using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using DRE.Interfaces;
using DRE.Libs.Lng.Models;
using DRE.Services;
using DRE.Views;
using Uno.Extensions.Navigation;

namespace DRE.ViewModels
{
    public class SetupDREViewModel : VM
    {

        private INavigator Navigator { get; }

        public int Width { get; set; }

        public int Height { get; set; }

     
        private readonly ISetupSvc _setupSvc;


        private String _lc;
        public String LanguageCode
        {
            get { return _lc; }
            set { SetProperty(ref _lc, value); }
        }


        public SetupDREViewModel(ISetupSvc setupSvc, INavigator navigator, ConfigSvc c)
        {
            Width = c.config.SavedWidth;

            Height = c.config.SavedHeight;

            Navigator = navigator;

         
        
            _setupSvc = setupSvc;

            LanguageCode = _setupSvc.SelectedLanguageCode();

        }

     


    }
}
