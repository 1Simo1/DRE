using DRE.Interfaces;
using DRE.Libs.Setup.Models;
using DRE.Libs.Trk.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace DRE.ViewModels
{
    public class TrkViewModel : VM
    {
        /// <summary>
        /// Track Operation % String
        /// </summary>
        private String _mp;
        public String msg
        {
            get { return _mp; }
            set { SetProperty(ref _mp, value); }
        }

        /// <summary>
        /// Track Operation % progressbar value
        /// </summary>
        private float _p;
        public float p
        {
            get { return _p; }
            set { SetProperty(ref _p, value); }
        }

        private ObservableCollection<TrkFile> _trkList;
        public ObservableCollection<TrkFile> trkList
        {
            get { return _trkList; }
            set { SetProperty(ref _trkList, value); }
        }

        private TrkFile _strk;
        public TrkFile SelectedTRK
        {
            get { return _strk; }
            set { 
                if (value!=null && value!=_strk)
                {
                    LoadTrack(value.id);
                }
                
                SetProperty(ref _strk, value); 
            
            }
        }

        private IProgress<float> x { get; set; }

        private readonly ISvcDRE _svc;

        public TrkViewModel(ISvcDRE svc)
        {
            _svc = svc;

            trkList = new ObservableCollection<TrkFile>(_svc.trackList());

        }

        private async void LoadTrack(int id)
        {
            try
            {
                x = new Progress<float>(value =>
                {
                    p = value;
                    msg = Int32.Parse($"{value}") + "%";

                });

                TrkInfo test = await Task.Run(() => _svc.LoadTrack(id, x));
                x.Report(100);
            }
            catch (Exception) { return; }
        }



    }
}
