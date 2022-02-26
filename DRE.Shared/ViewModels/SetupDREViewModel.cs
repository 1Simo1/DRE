using CommunityToolkit.Mvvm.Input;
using DRE.Interfaces;
using DRE.Libs.Setup.Models;
using DRE.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
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


        /// <summary>
        /// DRE Project Name
        /// </summary>
        private String p_dre;
        public String prj_dre
        {
            get { return p_dre; }
            set
            {
                SetProperty(ref p_dre, value);
                if (ConfProjectNameCmd != null) ConfProjectNameCmd.NotifyCanExecuteChanged();
            }
        }

        public IRelayCommand ConfProjectNameCmd { get; }

        /// <summary>
        /// Search for valid folder % String
        /// </summary>
        private String _mpf;
        public String msgf
        {
            get { return _mpf; }
            set { SetProperty(ref _mpf, value); }
        }

        /// <summary>
        /// Search for valid folder % progressbar value
        /// </summary>
        private float _pf;
        public float pf
        {
            get { return _pf; }
            set { SetProperty(ref _pf, value); }
        }

        /// <summary>
        /// Valid game folders found
        /// </summary>
        private ObservableCollection<String> _fl;
        public ObservableCollection<String> FolderList
        {
            get { return _fl; }
            set { SetProperty(ref _fl, value); }
        }

        private String _c_dre;
        public String SelectedGameFolder
        {
            get { return _c_dre; }
            set
            {
                SetProperty(ref _c_dre, value);
                SetupCmd.NotifyCanExecuteChanged();
            }
        }

        public IRelayCommand SetupCmd { get; }

        /// <summary>
        /// Setup % String
        /// </summary>
        private String _mp;
        public String msg
        {
            get { return _mp; }
            set { SetProperty(ref _mp, value); }
        }

        /// <summary>
        /// Setup % progressbar value
        /// </summary>
        private float _p;
        public float p
        {
            get { return _p; }
            set { SetProperty(ref _p, value); }
        }

        private String _setupMsg;
        public String SetupMsg
        {
            get { return _setupMsg; }
            set { SetProperty(ref _setupMsg, value); }
        }

        private String _sc;
        public String SetupCompletedVisibility
        {
            get { return _sc; }
            set { SetProperty(ref _sc, value); }
        }



        public SetupDREViewModel(ISetupSvc setupSvc, INavigator navigator, ConfigSvc c)
        {
            Width = c.config.SavedWidth;

            Height = c.config.SavedHeight;

            Navigator = navigator;

            _setupSvc = setupSvc;

            LanguageCode = _setupSvc.SelectedLanguageCode();

            prj_dre = String.Empty;

            ConfProjectNameCmd = new RelayCommand(ExecuteConfProjectNameCmd, CanExecuteConfProjectNameCmd);

            FolderList = new ObservableCollection<String>();

            SetupCmd = new RelayCommand(Setup, CanSetup);

            SetupCompletedVisibility = "Collapsed";

        }



        private bool CanExecuteConfProjectNameCmd()
        {
            return !String.IsNullOrEmpty(prj_dre.Trim());
        }

        private void ExecuteConfProjectNameCmd()
        {

            var x = new Progress<SetupProgress>(value =>
                                               {
                                                   pf = value.p;
                                                   msgf = Int32.Parse($"{value.p}") + "%";
                                                   if (value.msg != null) FolderList.Add(value.msg);
                                               });

            Task.Run(() => searchValidGameFolders(x));

        }

        private async Task<List<String>> searchValidGameFolders(IProgress<SetupProgress> x)
        {
            var df = new List<String>();

            var ld = new List<String>();

            try
            {

                x.Report(new SetupProgress() { p = 1 });

                foreach (var path in DriveInfo.GetDrives())
                {
                    ld.AddRange(Directory.GetDirectories(path.RootDirectory.FullName));
                }
                int n = 0;
                foreach (var path in ld)
                {
                    n++;
                    try
                    {
                        var test = Directory.GetFiles(path,
                      "DR.exe",
                      SearchOption.AllDirectories);

                        if (test.Length != 0)
                        {
                            foreach (var dir in test)
                            {
                                df.Add(dir.Substring(0, dir.LastIndexOf(Path.DirectorySeparatorChar)));
                                x.Report(new SetupProgress() { msg = df[df.Count - 1], p = n * 100 / ld.Count });
                            }

                        }
                        x.Report(new SetupProgress() { p = n * 100 / ld.Count });
                    }
                    catch (Exception e) { }

                }

            }
            catch (Exception e) { }

            x.Report(new SetupProgress() { p = 100 });

            return df;
        }

        private bool CanSetup()
        {
            return !String.IsNullOrEmpty(SelectedGameFolder?.Trim());
        }

        private void Setup()
        {

            var x = new Progress<SetupProgress>(value =>
            {
                p = value.p;
                msg = Int32.Parse($"{value.p}") + "%";
                if (value.msg != null) SetupMsg = value.msg;
                if (value.p == 100) SetupCompletedVisibility = "Visible";
            });

            Task.Run(() => _setupSvc.SetupNewProject(prj_dre.Trim(), SelectedGameFolder, x));
        }



    }
}
