using Dapper;
using DRE.Libs.Trk.Models;

namespace DRE.Libs.Trk
{
    public class LibTrk
    {
        private String dir { get; set; }
        public LibTrk(String gameFolderPath)
        {
            dir = gameFolderPath;
        }

        public List<TrkFile> Init()
        {
            List<TrkFile> trk = new List<TrkFile>();

            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "Suburbia", trNumber = 1, IsFlipped = false });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "Downtown", trNumber = 2, IsFlipped = false });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "Utopia", trNumber = 3, IsFlipped = false });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "Rock Zone", trNumber = 4, IsFlipped = false });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "Snake Alley", trNumber = 5, IsFlipped = false });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "Oasis", trNumber = 6, IsFlipped = false });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "Velodrome", trNumber = 7, IsFlipped = false });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "Holocaust", trNumber = 8, IsFlipped = false });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "Bogota", trNumber = 9, IsFlipped = false });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "West End", trNumber = 1, IsFlipped = true });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "Newark", trNumber = 2, IsFlipped = true });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "Complex", trNumber = 3, IsFlipped = true });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "Hell Mountain", trNumber = 4, IsFlipped = true });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "Desert Run", trNumber = 5, IsFlipped = true });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "Palm Side", trNumber = 6, IsFlipped = true });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "Eidolon", trNumber = 7, IsFlipped = true });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "Toxic Dump", trNumber = 8, IsFlipped = true });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "Borneo", trNumber = 9, IsFlipped = true });
            trk.Add(new TrkFile() { id = trk.Count + 1, Name = "The Arena", trNumber = 0, IsFlipped = false });

            return trk;
        }

        public List<TrkRecord> defaultTrackRecords()
        {
            List<TrkRecord> crd = new List<TrkRecord>();

            String[] cars = new String[6] { "VAGABOND", "DERVISH", "SENTINEL", "SHRIEKER", "WRAITH", "DELIVERATOR" };

            if (File.Exists($"{dir}/dr.cfg"))
            {
                byte[] file = System.IO.File.ReadAllBytes($"{dir}/dr.cfg");
                using (MemoryStream m = new MemoryStream(file))
                {
                    using (BinaryReader b = new BinaryReader(m))
                    {
                        b.ReadBytes(86);
                        byte[] rd;
                        string nm;
                        Int16 t;
                        string c;
                        for (short i = 1; i <= 108; i++)
                        {
                            rd = b.ReadBytes(12);
                            nm = "";
                            for (int n = 0; n < rd.Length; n++) { if (rd[n] != 0) nm += (char)rd[n]; }
                            t = (Int16)(b.ReadInt32() * 6000 + b.ReadInt32() * 100 + b.ReadInt32());
                            c = cars[(int)(Math.Floor((decimal)(i - 1) / 18))];

                            crd.Add(new TrkRecord()
                            {
                                Id = i,
                                TrkId = (int)(Math.Floor((decimal)(i - 1) % 18)) + 1,
                                Car = c,
                                Name = nm,
                                LapTime = t
                            });


                        }
                    }
                }
            }


            return crd;

        }

        public TrkInfo LoadTrack(int trackID, IProgress<float> x, System.Data.IDbConnection db)
        {
            x.Report((300 + trackID) / 14);

            TrkInfo trkInfo = new TrkInfo();

            TrkFile trk = db.Query<TrkFile>("SELECT id,nf AS Name,tr AS trNumber, f AS IsFlipped FROM trk WHERE id=@id", new { id = trackID }).Single();

            int trkInfosCount = db.Query("SELECT * FROM trk_info WHERE trk=@id", new { id = trackID }).ToList().Count;

            if (trkInfosCount != 127) //Track infos not yet loaded in DB
            {
                var trkInfoData = db.Query<Byte[]>
                    ("SELECT d FROM bpa_files WHERE nf=@nf", new { nf = $"TR{trk.trNumber}-INF.BIN" }).Single().AsList();


                for (int i = 1; i <= 127; i++)
                {
                    int v = BitConverter.ToInt32(trkInfoData.GetRange(4 * (i - 1), 4).ToArray(), 0);

                    db.Query("INSERT INTO trk_info(trk,a,v) VALUES(@trk,@a,@v)", new
                    {
                        trk = trackID,
                        a = i,
                        v = v
                    });

                    x.Report((300 + ((12 * i / 127) * trackID)) / 14);
                }

            }

            int trkFilesCount = db.Query("SELECT * FROM trk_files WHERE trk=@id", new { id = trackID }).ToList().Count;

            if (trkFilesCount == 0) // Track files not yet loaded in DB
            {
                //SCE file id in DB
                var sce = db.Query<int>("SELECT exp FROM bpa_files WHERE nf=@nf", new { nf = $"TR{trk.trNumber}-SCE.BPK" }).First();

                //SCE data for selected track
                List<byte>? d = db.Query<Byte[]>("SELECT d FROM exp WHERE id=@exp", new { exp = sce }).First().ToList();

                x.Report((300 + (12 * trackID)) / 14);

                db.Query("REPLACE INTO trk_files(trk,NF,n,d) VALUES(@trk,@nf,@n,@d)", new
                {
                    trk = trk.id,
                    nf = $"TR{trk.trNumber}-3D-HEADER",
                    n = 0,
                    d = d[0]
                });

                int total3DObjects = (int)d[0];

                for (int i = 1; i <= total3DObjects; i++)
                {
                    db.Query("REPLACE INTO trk_files(trk,NF,n,d) VALUES(@trk,@nf,@n,@d)", new
                    {
                        trk = trk.id,
                        nf = $"TR{trk.trNumber}-3D-HEADER",
                        n = i,
                        d = d.GetRange(((i - 1) * 3152) + 1, 3152)
                    });

                    /**
                     * More 3DObject computation here?
                     */

                    x.Report((300 + ((12 + (9 * i / total3DObjects)) * trackID)) / 14);
                }

                int delta = 3152 * total3DObjects + 1;

                d = d.GetRange(delta, d.Count - delta);

                int totalTextures = (int)d[0];

                db.Query("REPLACE INTO trk_files(trk,NF,n,d) VALUES(@trk,@nf,@n,@d)", new
                {
                    trk = trk.id,
                    nf = $"TR{trk.trNumber}-TEXTURE-HEADER",
                    n = 0,
                    d = d[0]
                });


                for (int i = 1; i <= totalTextures; i++)
                {

                    var t = d.GetRange(((i - 1) * 44) + 1, 44);

                    db.Query("REPLACE INTO trk_files(trk,NF,n,d) VALUES(@trk,@nf,@n,@d)", new
                    {
                        trk = trk.id,
                        nf = $"TR{trk.trNumber}-TEXTURE-HEADER",
                        n = i,
                        d = t
                    });


                    var txr = new TrkTexture()
                    {
                        width = BitConverter.ToInt32(t.GetRange(0, 4).ToArray(), 0),
                        height = BitConverter.ToInt32(t.GetRange(4, 4).ToArray(), 0),
                        offset = BitConverter.ToInt32(t.GetRange(8, 4).ToArray(), 0),
                        mapX = BitConverter.ToInt32(t.GetRange(12, 4).ToArray(), 0),
                        mapY = BitConverter.ToInt32(t.GetRange(16, 4).ToArray(), 0),
                        size_multiplier = BitConverter.ToInt32(t.GetRange(20, 4).ToArray(), 0),
                        realX = BitConverter.ToInt32(t.GetRange(24, 4).ToArray(), 0),
                        realY = BitConverter.ToInt32(t.GetRange(28, 4).ToArray(), 0),
                        Unknown_Unused = BitConverter.ToInt32(t.GetRange(32, 4).ToArray(), 0),
                        Linked3DObj = BitConverter.ToInt32(t.GetRange(36, 4).ToArray(), 0),
                        Linked3DObjPolygon = BitConverter.ToInt32(t.GetRange(40, 4).ToArray(), 0),
                    };

                    txr.Data = d.GetRange(txr.offset, txr.width * txr.height).ToArray();

                    db.Query("REPLACE INTO trk_files(trk,NF,n,d) VALUES(@trk,@nf,@n,@d)", new
                    {
                        trk = trk.id,
                        nf = $"TR{trk.trNumber}-TEXTURE",
                        n = i,
                        d = txr.Data
                    });



                    x.Report((300 + ((12 + (9 * i / total3DObjects)) * trackID)) / 14);
                }


                if (true) { }


            }

            trkInfo.fullImageWidth = db.Query<int>("SELECT v FROM trk_info WHERE trk=@id AND a=1", new { id = trackID }).FirstOrDefault();
            trkInfo.fullImageHeight = db.Query<int>("SELECT v FROM trk_info WHERE trk=@id AND a=2", new { id = trackID }).FirstOrDefault();
            trkInfo.totalZones = db.Query<int>("SELECT v FROM trk_info WHERE trk=@id AND a=3", new { id = trackID }).FirstOrDefault();

            trkInfo.driverStartPositions = new();

            for (int d = 1; d <= 4; d++)
            {
                trkInfo.driverStartPositions.Add(new()
                {
                    X = db.Query<int>("SELECT v FROM trk_info WHERE trk=@id AND a=@a", new { id = trackID, a = (d * 3) + 1 }).FirstOrDefault(),
                    Y = db.Query<int>("SELECT v FROM trk_info WHERE trk=@id AND a=@a", new { id = trackID, a = (d * 3) + 2 }).FirstOrDefault(),
                    Rotation = db.Query<int>("SELECT v FROM trk_info WHERE trk=@id AND a=@a", new { id = trackID, a = (d * 3) + 3 }).FirstOrDefault()
                });
            }

            trkInfo.powerUpPositions = new();

            for (int p = 1; p <= 16; p++)
            {
                trkInfo.powerUpPositions.Add(new()
                {
                    X = db.Query<int>("SELECT v FROM trk_info WHERE trk=@id AND a=@a", new { id = trackID, a = (p * 2) + 14 }).FirstOrDefault(),
                    Y = db.Query<int>("SELECT v FROM trk_info WHERE trk=@id AND a=@a", new { id = trackID, a = (p * 2) + 15 }).FirstOrDefault()
                });
            }

            trkInfo.pedestrianPositions = new();

            for (int p = 0; p < 20; p++)
            {
                trkInfo.pedestrianPositions.Add(new()
                {
                    X = db.Query<int>("SELECT v FROM trk_info WHERE trk=@id AND a=@a", new { id = trackID, a = (p * 4) + 48 }).FirstOrDefault(),
                    Y = db.Query<int>("SELECT v FROM trk_info WHERE trk=@id AND a=@a", new { id = trackID, a = (p * 4) + 49 }).FirstOrDefault(),
                    Type = db.Query<int>("SELECT v FROM trk_info WHERE trk=@id AND a=@a", new { id = trackID, a = (p * 4) + 50 }).FirstOrDefault(),
                    Rotation = db.Query<int>("SELECT v FROM trk_info WHERE trk=@id AND a=@a", new { id = trackID, a = (p * 4) + 51 }).FirstOrDefault()
                });
            }


            x.Report((300 + (30 * trackID)) / 14);


            return trkInfo;
        }

    }
}