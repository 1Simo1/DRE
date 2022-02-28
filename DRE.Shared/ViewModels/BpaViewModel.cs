using CommunityToolkit.Mvvm.Input;
using DRE.Interfaces;
using DRE.Libs.Bpa.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DRE.ViewModels
{
    public class BpaViewModel : VM
    {

        public IRelayCommand ExtImgsCmd { get; }

        /// <summary>
        /// Images from BPAs extraction % String
        /// </summary>
        private String _mp;
        public String msg
        {
            get { return _mp; }
            set { SetProperty(ref _mp, value); }
        }

        /// <summary>
        /// Images from BPAs extraction % progressbar value
        /// </summary>
        private float _p;
        public float p
        {
            get { return _p; }
            set { SetProperty(ref _p, value); }
        }

        private ObservableCollection<BpaFile> _bpaList;
        public ObservableCollection<BpaFile> bpaList
        {
            get { return _bpaList; }
            set { SetProperty(ref _bpaList, value); }
        }

        private BpaFile _sb;
        public BpaFile SelectedBPA
        {
            get { return _sb; }
            set { 
               
                if (value!=null && _sb!=value)
                {
                    bpaEntryList = new ObservableCollection<BpaFileEntry>(_svc.BpaFileList(value.id));
                }

                SetProperty(ref _sb, value);
            }
        }

        private ObservableCollection<BpaFileEntry> _singleBPA_File_Entry_List;
        public ObservableCollection<BpaFileEntry> bpaEntryList
        {
            get { return _singleBPA_File_Entry_List; }
            set { SetProperty(ref _singleBPA_File_Entry_List, value); }
        }

        private BpaFileEntry _bpaf;
        public BpaFileEntry bpaFile
        {
            get { return _bpaf; }
            set {

                if (value != null && _bpaf != value)
                {
                    OpList = new ObservableCollection<String>(_svc.computeBpaFileEntryAvailaibleOperations(value));
                }

                SetProperty(ref _bpaf, value); 
            
            }
        }

        private ObservableCollection<String> _bpaFileAvailableOperationList;
        public ObservableCollection<String> OpList
        {
            get { return _bpaFileAvailableOperationList; }
            set { SetProperty(ref _bpaFileAvailableOperationList, value); }
        }


        private readonly ISvcDRE _svc;
        public BpaViewModel(ISvcDRE svc)
        {
            _svc = svc;
            ExtImgsCmd = new RelayCommand(ExtractImagesFromBPAs);

            bpaList = new ObservableCollection<BpaFile>(svc.ListBpa());

        }

        private void ExtractImagesFromBPAs()
        {
            //throw new NotImplementedException();
        }
    }
}
