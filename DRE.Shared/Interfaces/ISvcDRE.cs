using DRE.Libs.Bpa.Models;
using DRE.Libs.SaveGame.Models;
using DRE.Libs.Setup;
using DRE.Libs.Setup.Models;
using DRE.Libs.Trk.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DRE.Interfaces
{
    public interface ISvcDRE
    {
        IDbConnection db { get => LibSetup.db; }
        List<BpaFile> ListBpa();
        List<BpaFileEntry> BpaFileList(int id);
        List<String> computeBpaFileEntryAvailaibleOperations(BpaFileEntry value);
        void bpaFileEntryOperation(BpaFileEntry bpaFile, String opCode, IProgress<SetupProgress> x);
        void WriteBPA(BpaFile selectedBPA, IProgress<SetupProgress> x);
        void ExtractImagesFromBPAs(IProgress<SetupProgress> x);
        List<SaveGameEntry> saveGameList();
        SaveGameInfo SaveGameInfo(string fileName);
        List<SaveGameEntry> SaveGameDriverList(string fileName);
        DriverInfo SaveGameDriverDetails(string fileName, int p);
        void SaveGameUpdateDriverDetails(DriverInfo driverDetails, SaveGameInfo info, SaveGameEntry DriverInfo);
        void SaveGameWriteFile(string fileName);
        void UpdateSaveGamesFromGameFolder();
        List<TrkFile> trackList();
        Task<TrkInfo> LoadTrack(int id, IProgress<float> x);

    }
}
