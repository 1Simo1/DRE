﻿using DRE.Libs.Bpa.Models;
using DRE.Libs.Setup;
using DRE.Libs.Setup.Models;
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
        List<String> computeBpaFileEntryAvailaibleOperations(BpaFileEntry value);
        void bpaFileEntryOperation(BpaFileEntry bpaFile, String opCode, IProgress<SetupProgress> x);
        void WriteBPA(BpaFile selectedBPA, IProgress<SetupProgress> x);
    }
}
