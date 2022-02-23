using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using DRE.Interfaces;
using DRE.Libs.Lng.Models;
using DRE.Services;
using Uno.Extensions.Navigation;

namespace DRE.ViewModels
{
    public class SetupDREViewModel : VM
    {

        private INavigator Navigator { get; }

        public int Width { get; set; }

        public int Height { get; set; }

        public IRelayCommand<String> SetLngCmd { get; }

        private ObservableCollection<Localization> _lngList;
        private readonly ISetupSvc _setupSvc;

        public ObservableCollection<Localization> LngList
        {
            get { return _lngList; }
            set { SetProperty(ref _lngList, value); }
        }

        public SetupDREViewModel(ISetupSvc setupSvc, INavigator navigator, ConfigSvc c)
        {
            Width = c.config.SavedWidth;

            Height = c.config.SavedHeight;

            Navigator = navigator;

            LngList = new ObservableCollection<Localization>(setupSvc.LoadLngList());
            SetLngCmd = new RelayCommand<String>(SetupLanguage);
        
            _setupSvc = setupSvc;
        }

        private void SetupLanguage(String languageCode)
        {
            
        }
    }
}
