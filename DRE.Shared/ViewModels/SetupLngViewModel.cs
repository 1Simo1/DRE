using CommunityToolkit.Mvvm.Input;
using Dapper;
using DRE.Interfaces;
using DRE.Libs.Lng.Models;
using DRE.Services;
using System;
using System.Collections.ObjectModel;
using Uno.Extensions.Navigation;

namespace DRE.ViewModels
{
    public class SetupLngViewModel : VM
    {
        private INavigator Navigator { get; }

        public int Width { get; set; }

        public int Height { get; set; }

        public IRelayCommand<String> SetLngCmd { get; }

        private ObservableCollection<Localization> _lngList;
        private readonly ISetupSvc _setupSvc;
        private readonly ConfigSvc _c;

        public ObservableCollection<Localization> LngList
        {
            get { return _lngList; }
            set { SetProperty(ref _lngList, value); }
        }



        public SetupLngViewModel(INavigator navigator, ISetupSvc setupSvc, ConfigSvc c)
        {

            Width = c.config.SavedWidth;

            Height = c.config.SavedHeight;

            Navigator = navigator;

            LngList = new ObservableCollection<Localization>(setupSvc.LoadLngList());
            SetLngCmd = new RelayCommand<String>(SetupLanguage);

            _setupSvc = setupSvc;
            _c = c;
        }

        private void SetupLanguage(String languageCode)
        {
            _setupSvc.SetupLanguage(languageCode);

            DRE.Libs.Setup.LibSetup.db.Query("UPDATE DRE SET v=@v WHERE n='defaultLanguage'", new { v = languageCode });

            Navigator.NavigateViewModelAsync<SetupDREViewModel>(this);

        }


    }
}
