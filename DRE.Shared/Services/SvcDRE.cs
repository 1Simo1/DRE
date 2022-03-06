using Dapper;
using DRE.Interfaces;
using DRE.Libs.Bpa.Models;
using DRE.Libs.SaveGame.Models;
using DRE.Libs.Setup;
using DRE.Libs.Setup.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DRE.Services
{
    public class SvcDRE : ISvcDRE
    {
        public IDbConnection db { get => LibSetup.db; }

        private SvcBPA _bpaSvc { get; set; }

        private SvcSG _sgSvc { get; set; }

        public SvcDRE()
        {
            _bpaSvc = new SvcBPA(db);
            _sgSvc = new SvcSG(db);
        }

        public List<BpaFile> ListBpa() => _bpaSvc.ListBpa();

        public List<BpaFileEntry> BpaFileList(int id) => _bpaSvc.BpaFileList(id);


        public List<String> computeBpaFileEntryAvailaibleOperations(BpaFileEntry value) => _bpaSvc.computeBpaFileEntryAvailaibleOperations(value);
        

        public async void bpaFileEntryOperation(BpaFileEntry bpaFile, String opCode, IProgress<SetupProgress> x)
        {
            await Task.Run(() => _bpaSvc.bpaFileEntryOperation(bpaFile, opCode, x));
        }

        public async void WriteBPA(BpaFile selectedBPA, IProgress<SetupProgress> x)
        {
            await Task.Run(() => _bpaSvc.WriteBPA(selectedBPA,x));
        }

        public async void ExtractImagesFromBPAs(IProgress<SetupProgress> x)
        {
            await Task.Run(() => _bpaSvc.ExtractImagesFromBPAs(x));
        }

        public List<SaveGameEntry> saveGameList() => _sgSvc.List();

        public SaveGameInfo SaveGameInfo(String fileName) => _sgSvc.SaveGameInfo(fileName);

        public List<SaveGameEntry> SaveGameDriverList(string fileName) => _sgSvc.SaveGameDriverList(fileName);

        public DriverInfo SaveGameDriverDetails(string fileName, int p) => _sgSvc.SaveGameDriverDetails(fileName,p);

        public void SaveGameUpdateDriverDetails(DriverInfo driverDetails, SaveGameInfo info, SaveGameEntry DriverInfo) => 
            _sgSvc.SaveGameUpdateDriverDetails(driverDetails,info,DriverInfo);

        public void SaveGameWriteFile(string fileName) => _sgSvc.Write(fileName);

        public void UpdateSaveGamesFromGameFolder() => _sgSvc.UpdateSaveGamesFromGameFolder();
       
    }
}
