using System.Data;
using Dapper;
using System.Data.SQLite;
using DRE.Libs.Setup.Models;
using DRE.Libs.Lng;
using DRE.Libs.Bpa;
using DRE.Libs.Bpa.Models;
using DRE.Libs.Trk;
using DRE.Libs.Trk.Models;
using DRE.Libs.Cfg;
using DRE.Libs.Cfg.Models;
using DRE.Libs.Haf;
using DRE.Libs.Haf.Models;
using DRE.Libs.SaveGame;
using DRE.Libs.SaveGame.Models;
using DRE.Libs.Lng.Models;
using System.Xml;

namespace DRE.Libs.Setup
{
    public class LibSetup
    {

        public String p_dre { get; set; }
        public String c_dre { get; set; }
        public String dre_v = "0.21";

        String cs = $"Data Source={AppDomain.CurrentDomain.BaseDirectory}db/DRE.db";

        private LibLng T;
        private LibBpa Bpa;
        private LibTrk Trk;
        private LibCfg Cfg;
        private LibHaf Haf;
        private SaveGameLib Sg;

        public static IDbConnection db = null;

        public int tot_tabs = 14;
        public LibSetup()
        {
            testDir($"{AppDomain.CurrentDomain.BaseDirectory}db");
            T = new LibLng();
            Setup("", "", null);
        }

        private String testDir(String dir)
        {
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return dir;
        }

        public bool testSetup() => Init() == tot_tabs;

        /// <summary>
        /// Counts total number of tables in DRE DB
        /// </summary>
        /// <returns>Total number of tables in DRE DB</returns>
        private int Init()
        {
            try {
                
                if (db == null) db = new SQLiteConnection($"Data Source={AppDomain.CurrentDomain.BaseDirectory}db/DRE.db");
                return db.Query<int>("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite%'").First();
            
            } catch (Exception ex) { return 0; }
        }

        public async Task Setup(String p_dre, String c_dre, IProgress<SetupProgress> x)
        {
            int i = Init();

            try
            {
                db = new SQLiteConnection(cs);


                if (i <= 1)
                {
                    db.Query("CREATE TABLE IF NOT EXISTS dre(id INTEGER PRIMARY KEY AUTOINCREMENT, n TEXT NOT NULL UNIQUE, v TEXT)");

                  

                    if (String.IsNullOrEmpty(p_dre)) {


                        db.Query("INSERT INTO dre(n,v) VALUES (@n,@v)", new { n = "dre_v", v = dre_v });

                        String dir = $"{AppDomain.CurrentDomain.BaseDirectory}db/locale";
                        XmlReader d = XmlReader.Create($"{dir}/default.xml");
                        d.Read(); // <xml>
                        d.Read(); // <default>
                        d.Read(); // Default Language Code
                        db.Query("INSERT INTO dre(n,v) VALUES (@n,@v)", new { n = "defaultLanguage", v = d.Value });
                        d.Close();
                    }
                    if (String.IsNullOrEmpty(p_dre)) return;

                    db.Query("INSERT INTO dre(n,v) VALUES (@n,@v)", new { n = "p_dre", v = p_dre });
                    db.Query("INSERT INTO dre(n,v) VALUES (@n,@v)", new { n = "c_dre", v = c_dre });
                   
                    x.Report(new SetupProgress() { msg = T._("setup"), p = i * 100 / tot_tabs });
              
                    //if (String.IsNullOrEmpty(p_dre)) return;

                    x.Report(new SetupProgress() { msg = T._("setup_bpa"), p = i * 100 / tot_tabs });

                    Bpa = new LibBpa();

                    var bpa_list = Bpa.List(c_dre);

                    db.Query("CREATE TABLE IF NOT EXISTS bpa(id INTEGER PRIMARY KEY, nf TEXT NOT NULL, dim INTEGER NOT NULL)");

                    int bpa_n = 1;
                    foreach (var nbpa in bpa_list)
                    { //BPA
                        db.Query("INSERT INTO bpa(id,nf,dim) VALUES(@id,@nf,@dim)", new
                        {
                            id = bpa_n,
                            nf = nbpa.Name,
                            dim = nbpa.Size
                        });
                        bpa_n++;
                    }

                    db.Query(
                        "CREATE TABLE IF NOT EXISTS bpa_files" +
                        "(id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                        "nf TEXT NOT NULL ," +
                        "bpa INTEGER DEFAULT NULL," +
                        "n INTEGER NOT NULL, " +
                        "dim INTEGER NOT NULL, " +
                        "pal INTEGER DEFAULT NULL, " +
                        "exp INTEGER DEFAULT NULL, " +
                        "d BLOB NOT NULL)"
                        );
                    bpa_n = 1;

                    foreach (var nbpa in bpa_list)
                    {
                        List<BpaFileEntry> bpaEntries = Bpa.BpaFileEntries(nbpa.Name);
                        int fn = 1;
                        foreach (var fbpa in bpaEntries)
                        {
                            db.Query("INSERT INTO bpa_files(nf,bpa,n,dim,d) VALUES(@nf,@bpa,@n,@dim,@d)", new
                            {
                                nf = fbpa.FileName,
                                bpa = bpa_n,
                                n = fn,
                                dim = fbpa.Size,
                                d = fbpa.Data
                            });
                            fn++;
                        }
                        bpa_n++;
                    }
                } // BPA

                if (String.IsNullOrEmpty(p_dre)) return;

                if (i <= 3)
                {

                    x.Report(new SetupProgress() { msg = T._("setup_cfg"), p = 300 / tot_tabs });

                    Trk = new LibTrk(c_dre);

                    List<TrkFile> trk_list = Trk.Init();

                    db.Query("CREATE TABLE IF NOT EXISTS trk(id INTEGER PRIMARY KEY, nf text NOT NULL, tr INTEGER NOT NULL, f BOOLEAN)");

                    foreach (var ntrk in trk_list)
                    {
                        db.Query("INSERT INTO trk(id,nf,tr,f) VALUES(@id,@nf,@tr,@f)", new
                        {
                            id = ntrk.Id,
                            nf = ntrk.Name,
                            tr = ntrk.trNumber,
                            f = ntrk.IsFlipped
                        });

                    }

                    db.Query("CREATE TABLE IF NOT EXISTS crd(id INTEGER PRIMARY KEY, trk INTEGER NOT NULL, c text NOT NULL, n text NOT NULL, t TEXT)");
                   // Dictionary<short, Tuple<int, string, string, Int16>> rd = Trk.defaultTrackRecords();

                    List<TrkRecord> rd = Trk.defaultTrackRecords();

                    foreach (var crd in rd)
                    {
                        db.Query("INSERT INTO crd VALUES (@id,@trk,@c,@n,@t)", new
                        {
                            id = crd.Id,
                            trk = crd.TrkId,
                            c = crd.Car,
                            n = crd.Name,
                            t = ((decimal)crd.LapTime / 100).ToString()
                        });
                    }

                    db.Query("CREATE TABLE IF NOT EXISTS trk_info(id INTEGER PRIMARY KEY, trk INTEGER NOT NULL,c text NOT NULL,v INTEGER)");

                    db.Query("CREATE TABLE IF NOT EXISTS trk_files(id INTEGER PRIMARY KEY, trk INTEGER NOT NULL,NF text NOT NULL,n INTEGER,d BLOB NOT NULL,UNIQUE(trk,NF,n))");

                    Cfg = new LibCfg(c_dre);

                    //Dictionary<short, Tuple<string, int, short>> cfg_hof = cfg.Hof();

                    List<HofEntry> cfg_hof = Cfg.Hof();

                    db.Query("CREATE TABLE IF NOT EXISTS cfg(id INTEGER PRIMARY KEY, info TEXT,v INTEGER)");

                    db.Query("CREATE TABLE IF NOT EXISTS hof(pos INTEGER PRIMARY KEY, n TEXT, tr INTEGER NOT NULL,lv INTEGER NOT NULL)");

                    foreach (var hof in cfg_hof)
                    {
                        db.Query("INSERT INTO hof VALUES (@id,@n,@tr,@lv)", new
                        {
                            id = hof.Id,
                            n = hof.Name,
                            tr = hof.TotalRaces,
                            lv = hof.Level
                        });
                    }



                } // CFG

                if (i <= 9)
                {
                    x.Report(new SetupProgress() { msg = T._("setup_haf"), p = 900 / tot_tabs });

                    Haf = new LibHaf(c_dre);

                    List<HafFile> lhaf = Haf.Init();

                    db.Query("CREATE TABLE IF NOT EXISTS haf(id INTEGER PRIMARY KEY, nf TEXT NOT NULL, n INTEGER NOT NULL,d BLOB)");

                    foreach (var fhaf in lhaf)
                    {
                        db.Query("INSERT INTO haf(id,nf,n,d) VALUES(@id,@nf,@n,@d)", new
                        {
                            id = fhaf.Id,
                            nf = fhaf.Name,
                            n = fhaf.FrameNumber,
                            d = fhaf.Data
                        });
                    }

                } // HAF


                if (i <= 10)
                {
                    x.Report(new SetupProgress() { msg = T._("setup_sg"), p = 1000 / tot_tabs });
                    db.Query("CREATE TABLE IF NOT EXISTS sg(id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,nf TEXT,p INTEGER,n INTEGER,v INTEGER,t TEXT, UNIQUE(nf,p,n))");

                    Sg = new SaveGameLib(c_dre);

                    List<SaveGameEntry> sgd = Sg.Init();


                    foreach (var sd in sgd)
                    {
                        db.Query("INSERT INTO sg(nf, p, n, v, t) VALUES(@nf, @p, @n, @v, @t)", new
                        {
                            nf = sd.FileName,
                            p = sd.Position,
                            n = sd.AttributeNumber,
                            v = sd.Value,
                            t = sd.ValueText
                        });
                    }
                } // SG

                if (i <= 11)
                {
                    x.Report(new SetupProgress() { msg = T._("setup_def"), p = 1100 / tot_tabs });

                    db.Query("CREATE TABLE IF NOT EXISTS exp(id INTEGER PRIMARY KEY,d BLOB,w INTEGER NOT NULL,h INTEGER NOT NULL,rix BOOLEAN)");
                    db.Query("CREATE TABLE IF NOT EXISTS pal(id INTEGER PRIMARY KEY,d BLOB)");
                    db.Query("CREATE TABLE IF NOT EXISTS cmf(id INTEGER PRIMARY KEY,nf TEXT,n INTEGER NOT NULL,d BLOB)");

                    x.Report(new SetupProgress() { msg = T._("setup_dr"), p = 100 * (tot_tabs - 1) / tot_tabs });

                    byte[] file = System.IO.File.ReadAllBytes(c_dre + "/dr.exe");

                    db.Query("INSERT INTO dre(n,v) VALUES (@n,@v)", new { n = "dr", v = Convert.ToBase64String(file) });

                    db.Close();
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + "/db/DRE.db", AppDomain.CurrentDomain.BaseDirectory + "/db/DEFAULT.db");
                    db = new SQLiteConnection(@"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "/db/DEFAULT.db");

                    db.Query("UPDATE dre set v=@p WHERE id=1", new { p = "DRE" });
                    db.Query("UPDATE dre set v=@c WHERE id=2", new { c = "default" });
                    db.Query("DELETE FROM sg");

                    db.Close();

                    db = new SQLiteConnection(cs);

                    x.Report(new SetupProgress() { msg = T._(""), p = 100 });

                } // DEFAULT DB & OK


                if (i == tot_tabs) x.Report(new SetupProgress() { msg = T._(""), p = 100 });

            }
            catch (Exception e) { }
        }

        public List<Localization> LngList() => T.LngList();

        public void SetupLanguage(String languageCode) => T.setLng(languageCode);

        public String SelectedLanguageCode() => T.SelectedLanguageCode();
        

    }
}