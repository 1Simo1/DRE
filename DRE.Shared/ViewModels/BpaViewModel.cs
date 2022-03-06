using CommunityToolkit.Mvvm.Input;
using DRE.Interfaces;
using DRE.Libs.Bpa.Models;
using DRE.Libs.Setup.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

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

        public IRelayCommand WriteBPACmd { get; }

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

#if HAS_UNO_SKIA

        private String _sop;
        public String SelectedOP
        {
            get { return _sop; }
            set { 
            if (value!=null && _sop != value)
                {
                    bpaFileEntryOperation(value);
                }
                SetProperty(ref _sop, value); 
            
            }
        }

#endif


        public IRelayCommand<String> bpaOpCmd { get; }


        private IProgress<SetupProgress> x { get; set; }

        private readonly ISvcDRE _svc;
        public BpaViewModel(ISvcDRE svc)
        {
            _svc = svc;

            ExtImgsCmd = new RelayCommand(ExtractImagesFromBPAs);


            bpaList = new ObservableCollection<BpaFile>(svc.ListBpa());

            WriteBPACmd = new RelayCommand(WriteBPA);

            bpaOpCmd = new RelayCommand<String>(bpaFileEntryOperation);

           
        }

      

        private async void ExtractImagesFromBPAs()
        {
            try
            {
                x = new Progress<SetupProgress>(value =>
                {
                    p = value.p;
                    msg = Int32.Parse($"{value.p}") + "%";

                });

                await Task.Run(() => _svc.ExtractImagesFromBPAs(x));
                x.Report(new SetupProgress() { p = 100 });
            }
            catch (Exception) { return; }

        }

        private async void WriteBPA()
        {
            try
            {
                x = new Progress<SetupProgress>(value =>
                {
                    p = value.p;
                    msg = Int32.Parse($"{value.p}") + "%";

                });

                await Task.Run(() => _svc.WriteBPA(SelectedBPA,x));
                x.Report(new SetupProgress() { p = 100 });
            }
            catch (Exception) { return; }

        }


        private async void bpaFileEntryOperation(String opCode)
        {
            try
            {
                x = new Progress<SetupProgress>(value =>
                {
                    p = value.p;
                    msg = Int32.Parse($"{value.p}") + "%";

                });

                await Task.Run(() => _svc.bpaFileEntryOperation(bpaFile, opCode,x));

                x.Report(new SetupProgress() { p = 100 });

            }
            catch (Exception) { return; }

           
        }


    }
}
