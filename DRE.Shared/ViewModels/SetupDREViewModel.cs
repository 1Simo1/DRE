using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using DRE.Interfaces;
using DRE.Libs.Lng.Models;

namespace DRE.ViewModels
{
    public class SetupDREViewModel : VM
    {
        public IRelayCommand<String> SetLngCmd { get; }

        private ObservableCollection<Localization> _lngList;
        private readonly ISetupSvc _setupSvc;

        public ObservableCollection<Localization> LngList
        {
            get { return _lngList; }
            set { SetProperty(ref _lngList, value); }
        }

        public SetupDREViewModel(ISetupSvc setupSvc)
        {
            LngList = new ObservableCollection<Localization>(setupSvc.LoadLngList());
            SetLngCmd = new RelayCommand<String>(SetupLanguage);
         
            


            _setupSvc = setupSvc;
        }

        private void SetupLanguage(String languageCode)
        {
            
        }
    }
}
