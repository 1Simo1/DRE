using CommunityToolkit.Mvvm.Input;
using DRE.Interfaces;
using DRE.Libs.SaveGame.Models;
using System;
using System.Collections.ObjectModel;

namespace DRE.ViewModels
{
    public class SaveGameViewModel : VM
    {

        private ObservableCollection<SaveGameEntry> _saveGameList;
        public ObservableCollection<SaveGameEntry> saveGameList
        {
            get { return _saveGameList; }
            set { SetProperty(ref _saveGameList, value); }
        }

        private SaveGameEntry _selectedSG;
        public SaveGameEntry SelectedSG
        {
            get { return _selectedSG; }
            set { 
               
                if (value != null && value!=SelectedSG)
                {
                    InfoSG = _svc.SaveGameInfo(value.FileName);
                    SaveGameDriverList = new ObservableCollection<SaveGameEntry>(_svc.SaveGameDriverList(value.FileName));

                }

                SetProperty(ref _selectedSG, value);
                WriteSGCmd.NotifyCanExecuteChanged();
            }
        }

        public IRelayCommand WriteSGCmd { get; }

        public IRelayCommand UpdateFromGameFolderCmd { get; }

        private SaveGameInfo _info;
        public SaveGameInfo InfoSG
        {
            get { return _info; }
            set { SetProperty(ref _info, value); }
        }

        private ObservableCollection<SaveGameEntry> _drl;
        public ObservableCollection<SaveGameEntry> SaveGameDriverList
        {
            get { return _drl; }
            set { SetProperty(ref _drl, value); }
        }

        private SaveGameEntry _dri;
        public SaveGameEntry DriverInfo
        {
            get { return _dri; }
            set {


                if (value != null && value != DriverInfo)
                {
                    _svc.SaveGameUpdateDriverDetails(DriverDetails,InfoSG,DriverInfo); //Updates previous selected driver data in DB
                    DriverDetails = _svc.SaveGameDriverDetails(value.FileName,value.Position);
                }

                SetProperty(ref _dri, value); 
            }
        }

        private DriverInfo _drd;
        public DriverInfo DriverDetails
        {
            get { return _drd; }
            set { SetProperty(ref _drd, value); }
        }


        private readonly ISvcDRE _svc;

        public SaveGameViewModel(ISvcDRE svc)
        {
            _svc = svc;

            saveGameList = new ObservableCollection<SaveGameEntry>(_svc.saveGameList());

            WriteSGCmd = new RelayCommand(SaveFile,CanSaveFile);

            UpdateFromGameFolderCmd = new RelayCommand(UpdateFromGameFolder);


        }

     

        private bool CanSaveFile() => SelectedSG != null;
    
        private void SaveFile()
        {
            _svc.SaveGameUpdateDriverDetails(DriverDetails,InfoSG,DriverInfo); //Updates previous selected driver data in DB

            _svc.SaveGameWriteFile(SelectedSG.FileName);

        }

        private void UpdateFromGameFolder() 
        {
            _svc.UpdateSaveGamesFromGameFolder();
            saveGameList = new ObservableCollection<SaveGameEntry>(_svc.saveGameList());
        }


    }
}
