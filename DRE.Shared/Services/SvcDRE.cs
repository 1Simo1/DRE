using Dapper;
using DRE.Interfaces;
using DRE.Libs.Bpa.Models;
using DRE.Libs.SaveGame.Models;
using DRE.Libs.Setup;
using DRE.Libs.Setup.Models;
using DRE.Libs.Trk.Models;
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

        private SvcTRK _trkSvc { get; set; }

        public SvcDRE()
        {
            _bpaSvc = new SvcBPA(db);
            _sgSvc = new SvcSG(db);
            _trkSvc = new SvcTRK(db);
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
            await Task.Run(() => _bpaSvc.WriteBPA(selectedBPA, x));
        }

        public async void ExtractImagesFromBPAs(IProgress<SetupProgress> x)
        {
            await Task.Run(() => _bpaSvc.ExtractImagesFromBPAs(x));
        }

        public List<SaveGameEntry> saveGameList() => _sgSvc.List();

        public SaveGameInfo SaveGameInfo(String fileName) => _sgSvc.SaveGameInfo(fileName);

        public List<SaveGameEntry> SaveGameDriverList(string fileName) => _sgSvc.SaveGameDriverList(fileName);

        public DriverInfo SaveGameDriverDetails(string fileName, int p) => _sgSvc.SaveGameDriverDetails(fileName, p);

        public void SaveGameUpdateDriverDetails(DriverInfo driverDetails, SaveGameInfo info, SaveGameEntry DriverInfo) =>
            _sgSvc.SaveGameUpdateDriverDetails(driverDetails, info, DriverInfo);

        public void SaveGameWriteFile(string fileName) => _sgSvc.Write(fileName);

        public void UpdateSaveGamesFromGameFolder() => _sgSvc.UpdateSaveGamesFromGameFolder();

        public List<TrkFile> trackList() => _trkSvc.List();

        public async Task<TrkInfo> LoadTrack(int id, IProgress<float> x)
        {

            /* Expand SCE and SHA track files, extract Track other BPK images */
            TrkFile trk = db.Query<TrkFile>("SELECT id,nf AS Name,tr AS trNumber, f AS IsFlipped FROM trk WHERE id=@id", new { id = id }).Single();

            var bpa = _bpaSvc.ListBpa().Where(x => x.Name.Equals($"TR{trk.trNumber}")).Single();

            var ima = _bpaSvc.BpaFileList(bpa.id).Where(x => x.FileName.Equals($"TR{trk.trNumber}-IMA.BPK")).Single();

            var mas = _bpaSvc.BpaFileList(bpa.id).Where(x => x.FileName.Equals($"TR{trk.trNumber}-MAS.BPK")).Single();

            var vai = _bpaSvc.BpaFileList(bpa.id).Where(x => x.FileName.Equals($"TR{trk.trNumber}-VAI.BPK")).Single();

            var lr1 = _bpaSvc.BpaFileList(bpa.id).Where(x => x.FileName.Equals($"TR{trk.trNumber}-LR1.BPK")).Single();

            var sce = _bpaSvc.BpaFileList(bpa.id).Where(x => x.FileName.Equals($"TR{trk.trNumber}-SCE.BPK")).Single();

            var sha = _bpaSvc.BpaFileList(bpa.id).Where(x => x.FileName.Equals($"TR{trk.trNumber}-SHA.BPK")).Single();

            bpa = _bpaSvc.ListBpa().Where(x => x.Name.Equals($"MENU")).Single();

            var shapeFileName = id < 10 ? $"TSHAPE0{id}.BPK" : $"TSHAPE{id}.BPK";

            var shape = _bpaSvc.BpaFileList(bpa.id).Where(x => x.FileName.Equals(shapeFileName)).Single();

            var raceSelection = _bpaSvc.BpaFileList(bpa.id).Where(x => x.FileName.Equals("TRSNAP2M.BPK")).Single();

            var arenaIntro = _bpaSvc.BpaFileList(bpa.id).Where(x => x.FileName.Equals("BADSNAP.BPK")).Single();

            _bpaSvc.bpkImg(ima, null, trk.IsFlipped, true);
            _bpaSvc.bpkImg(mas, null, false, true);
            _bpaSvc.bpkImg(vai, null, false, true);
            _bpaSvc.bpkImg(lr1, null, false, true);

            _bpaSvc.bpkImg(shape, null, false, true);

            if (trk.trNumber != 0)
            {
                _bpaSvc.bpaFileEntryOperation(raceSelection, "ext_img", new Progress<SetupProgress>());
            }
            else _bpaSvc.bpaFileEntryOperation(arenaIntro, "ext_img", new Progress<SetupProgress>());



            _bpaSvc.bpaFileEntryOperation(sce, "ext_bpk", new Progress<SetupProgress>());
            _bpaSvc.bpaFileEntryOperation(sha, "ext_bpk", new Progress<SetupProgress>());

            /* Load Track */
            return await Task.Run(() => _trkSvc.LoadTrack(id, x, db));
        }

        public void ExtractTrackTextures(TrkFile selectedTRK, IProgress<float> x) => _trkSvc.ExtractTrackTextures(selectedTRK, x);

    }
}
