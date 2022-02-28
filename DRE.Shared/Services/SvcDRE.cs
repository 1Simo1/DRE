using Dapper;
using DRE.Interfaces;
using DRE.Libs.Bpa.Models;
using DRE.Libs.Setup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DRE.Services
{
    public class SvcDRE : ISvcDRE
    {
        public IDbConnection db { get => LibSetup.db; }

        public SvcDRE()
        {

        }

        public List<BpaFile> ListBpa() => db.Query<BpaFile>("SELECT id,nf AS Name, dim AS Size FROM bpa ORDER BY id ASC").ToList();

        public List<BpaFileEntry> BpaFileList(int id)
        {
            return db.Query<BpaFileEntry>("SELECT id, nf AS FileName, bpa AS bpaID, n, dim AS Size, pal,exp, d as Data " +
                                          "FROM bpa_files WHERE bpa=@bpa_id ORDER BY n ASC", new { bpa_id = id }).ToList();
        }

        public List<String> computeBpaFileEntryAvailaibleOperations(BpaFileEntry value)
        {
            List<String> opList = new List<String>();

#if DEBUG
            opList.Add("ext_bpk");
            opList.Add("ext_img");
            opList.Add("ext_file");
            opList.Add("agg");
#endif

            return opList;
        }
    }
}
