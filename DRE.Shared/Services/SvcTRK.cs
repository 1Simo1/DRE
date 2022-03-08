using Dapper;
using DRE.Libs.Setup.Models;
using DRE.Libs.Trk.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRE.Services
{
    public class SvcTRK
    {
        private IDbConnection db { get; }

        private String gameFolder { get => db.Query<String>("SELECT v FROM DRE WHERE n='c_dre'").First(); }

        private IProgress<SetupProgress> x { get; set; }

        public SvcTRK(IDbConnection DRE_db) => db = DRE_db;

        public List<TrkFile> List() => db.Query<TrkFile>("SELECT id, nf AS Name, tr AS trNumber, f AS IsFlipped FROM trk ORDER BY id ASC").AsList();

        public async Task<TrkInfo> LoadTrack(int id, IProgress<float> x, IDbConnection db) => new Libs.Trk.LibTrk(gameFolder).LoadTrack(id, x, db);
       
    }
}
