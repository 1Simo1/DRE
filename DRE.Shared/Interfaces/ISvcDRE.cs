using DRE.Libs.Bpa.Models;
using DRE.Libs.Setup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DRE.Interfaces
{
    public interface ISvcDRE
    {
        IDbConnection db { get => LibSetup.db; }
        List<BpaFile> ListBpa();
        List<BpaFileEntry> BpaFileList(int id);
    }
}
