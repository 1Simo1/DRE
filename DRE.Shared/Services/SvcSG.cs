using Dapper;
using DRE.Libs.SaveGame;
using DRE.Libs.SaveGame.Models;
using DRE.Libs.Setup.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace DRE.Services
{
    public class SvcSG
    {
        private IDbConnection db { get; }

        private String gameFolder { get => db.Query<String>("SELECT v FROM DRE WHERE n='c_dre'").First(); }

        private IProgress<SetupProgress> x { get; set; }

        public SvcSG(IDbConnection DRE_db) => db = DRE_db;

        public List<SaveGameEntry> List()
        {
            return db.Query<SaveGameEntry>(
             "SELECT id, nf AS FileName, p AS Position, n AS AttributeNumber, v AS Value, t AS ValueText " +
             "FROM sg WHERE p=0 AND n=4"
             ).ToList();
        }

        public SaveGameInfo SaveGameInfo(String fileName)
        {
           var sg_info = db.Query<SaveGameEntry>(
             "SELECT id, nf AS FileName, p AS Position, n AS AttributeNumber, v AS Value, t AS ValueText " +
             "FROM sg WHERE nf=@nf AND p=0 ORDER BY n ASC", new {nf = fileName}).ToList();

            return new SaveGameInfo()
            {
                FileName = fileName,
                Key = sg_info[0].Value,
                PlayerIndex = sg_info[1].Value,
                Level = sg_info[2].Value,
                UseWeapons = sg_info[3].Value==1,
                SaveGameName = sg_info[4].ValueText
            };
        }

        public List<SaveGameEntry> SaveGameDriverList(string fileName)
        {
            return db.Query<SaveGameEntry>(
           "SELECT id, nf AS FileName, p AS Position, n AS AttributeNumber, v AS Value, t AS ValueText " +
           "FROM sg WHERE nf=@nf AND p!=0 AND n=0 ORDER BY p ASC" , new {nf = fileName}
           ).ToList();
        }

        public DriverInfo SaveGameDriverDetails(string fileName, int Position)
        {
            List<SaveGameEntry> fileDriverEntries = db.Query<SaveGameEntry>(
               "SELECT id, nf AS FileName, p AS Position, n AS AttributeNumber, v AS Value, t AS ValueText " +
               "FROM sg WHERE nf=@nf AND p=@pt ORDER BY n ASC", new { nf = fileName, pt = Position }
               ).ToList();

         
                return new DriverInfo()
                {
                    Name = fileDriverEntries.Find(x => x.AttributeNumber==0).ValueText,
                    Damage = fileDriverEntries.Find(x => x.AttributeNumber == 1).Value,
                    Engine = fileDriverEntries.Find(x => x.AttributeNumber == 2).Value,
                    Tyre = fileDriverEntries.Find(x => x.AttributeNumber == 3).Value,
                    Armour = fileDriverEntries.Find(x => x.AttributeNumber == 4).Value,
                    CarType = fileDriverEntries.Find(x => x.AttributeNumber == 5).Value,
                    Unknown_one = fileDriverEntries.Find(x => x.AttributeNumber == 6).Value,
                    Unknown_two = fileDriverEntries.Find(x => x.AttributeNumber == 7).Value,
                    Unknown_three = fileDriverEntries.Find(x => x.AttributeNumber == 8).Value,
                    Color = fileDriverEntries.Find(x => x.AttributeNumber == 9).Value,
                    Money = fileDriverEntries.Find(x => x.AttributeNumber == 10).Value,
                    LoanType = fileDriverEntries.Find(x => x.AttributeNumber == 11).Value,
                    LoanRacesLeft = fileDriverEntries.Find(x => x.AttributeNumber == 12).Value,
                    CarValue = fileDriverEntries.Find(x => x.AttributeNumber == 13).Value,
                    FaceId = fileDriverEntries.Find(x => x.AttributeNumber == 14).Value,
                    Points = fileDriverEntries.Find(x => x.AttributeNumber == 15).Value,
                    Rank = fileDriverEntries.Find(x => x.AttributeNumber == 16).Value,
                    Victories = fileDriverEntries.Find(x => x.AttributeNumber == 17).Value,
                    TotalRaces = fileDriverEntries.Find(x => x.AttributeNumber == 18).Value,
                    Unknown_four = fileDriverEntries.Find(x => x.AttributeNumber == 19).Value,
                    TotalIncome = fileDriverEntries.Find(x => x.AttributeNumber == 20).Value,
                    Mines = fileDriverEntries.Find(x => x.AttributeNumber == 21).Value,
                    Spikes = fileDriverEntries.Find(x => x.AttributeNumber == 22).Value==1,
                    Rocket = fileDriverEntries.Find(x => x.AttributeNumber == 23).Value==1,
                    Sabotage = fileDriverEntries.Find(x => x.AttributeNumber == 24).Value==1
                }; 
        }

        public void SaveGameUpdateDriverDetails(DriverInfo driverDetails, SaveGameInfo info, SaveGameEntry DriverInfo)
        {
            if (driverDetails==null || info==null || DriverInfo==null) return;

            try
            {
               String saveGameFileName = info.FileName;

               int p = db.Query<int>("SELECT p FROM sg WHERE nf=@nf AND n=0 AND t=@t", new { nf = saveGameFileName, t = DriverInfo.ValueText }).First();

                db.Query("UPDATE sg SET t=@v WHERE nf=@nf AND p=@np AND n=0", new { v = driverDetails.Name, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=1", new { v = driverDetails.Damage, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=2", new { v = driverDetails.Engine, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=3", new { v = driverDetails.Tyre, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=4", new { v = driverDetails.Armour, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=5", new { v = driverDetails.CarType, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=6", new { v = driverDetails.Unknown_one, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=7", new { v = driverDetails.Unknown_two, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=8", new { v = driverDetails.Unknown_three, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=9", new { v = driverDetails.Color, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=10", new { v = driverDetails.Money, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=11", new { v = driverDetails.LoanType, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=12", new { v = driverDetails.LoanRacesLeft, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=13", new { v = driverDetails.CarValue, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=14", new { v = driverDetails.FaceId, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=15", new { v = driverDetails.Points, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=16", new { v = driverDetails.Rank, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=17", new { v = driverDetails.Victories, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=18", new { v = driverDetails.TotalRaces, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=19", new { v = driverDetails.Unknown_four, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=20", new { v = driverDetails.TotalIncome, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=21", new { v = driverDetails.Mines, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=22", new { v = driverDetails.Spikes, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=23", new { v = driverDetails.Rocket, nf = saveGameFileName, np = p });
                db.Query("UPDATE sg SET v=@v WHERE nf=@nf AND p=@np AND n=24", new { v = driverDetails.Sabotage, nf = saveGameFileName, np = p });
            }
            catch (Exception ex) { return; }
        }

        public void Write(string fileName)
        {
            
            /// Save Game Data in DB 
            var sgd = db.Query<SaveGameEntry>(
             "SELECT id, nf AS FileName, p AS Position, n AS AttributeNumber, v AS Value, t AS ValueText " +
             "FROM sg WHERE nf=@nf ORDER BY p ASC, n ASC" , new {nf = fileName}
             ).ToList();

            List <Byte> saveGame = new();

            try
            {
                int k = sgd.Find(n => n.Position==0 && n.AttributeNumber==0).Value; //File Key

                saveGame.Add((byte) k);

                int p_id = sgd.Find(n => n.Position == 0 && n.AttributeNumber == 1).Value; //Player id
                int x = codeSaveGameByte(p_id, 1, k);
                saveGame.Add((byte)x);

                int w = sgd.Find(n => n.Position == 0 && n.AttributeNumber == 2).Value; //Weapons
                x = codeSaveGameByte(w, 2, k);
                saveGame.Add((byte)x);

                int df = sgd.Find(n => n.Position == 0 && n.AttributeNumber == 3).Value; //Difficulty
                x = codeSaveGameByte(df, 3, k);
                saveGame.Add((byte)x);

                String gameName = sgd.Find(n => n.Position == 0 && n.AttributeNumber == 4).ValueText;

                gameName = gameName.Length <= 15 ? gameName.PadRight(15, (char)0) : gameName.Substring(0, 15);

                for (int i = 0;i<15;i++)
                {
                    x = codeSaveGameByte((int) gameName[i], i+4, k);
                    saveGame.Add((byte)x);
                }


                /* Drivers Data */
                for (int d = 1;d<=20; d++)
                {
                    int sp = 19 + 108 * (d-1);
                    String driverName = sgd.Find(n => n.Position == d && n.AttributeNumber == 0).ValueText;
                    driverName = driverName.Length <= 12 ? driverName.PadRight(12, (char)0) : driverName.Substring(0, 12);

                        for (int i = 0; i < 12; i++)
                        {
                            x = codeSaveGameByte((int)driverName[i], sp+i, k);
                            saveGame.Add((byte)x);
                        }

                        for (int a=1; a<=24; a++)
                        {
                            int v = sgd.Find(n => n.Position == d && n.AttributeNumber == a).Value;
                        
                            var xd = BitConverter.GetBytes(v);

                            for (int i = 0;i<=3;i++)
                            {
                                x = codeSaveGameByte((int)xd[i], sp+12+((a-1)*4)+i, k);
                                saveGame.Add((byte)x);
                            }

                        }
                }

                File.WriteAllBytes($"{gameFolder}/{fileName}", saveGame.ToArray());
            }
            catch (Exception e) { return; }
        }

        private int codeSaveGameByte(int byteValue, int filePosition, int key)
        {
            int x = byteValue;
            int r = filePosition % 6;
            x = x - key;
            x %= 256;
            x += 17 * filePosition;
            x %= 256;
            var t = x >> filePosition % 6;
            x %= 256;
            x = t | x << (8 - filePosition % 6);
            x %= 256;

            if (x < 0) x += 256;

            return x;
        }


        public void UpdateSaveGamesFromGameFolder()
        {

            db.Query("DELETE FROM sg");

            var Sg = new SaveGameLib(gameFolder);

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
        }

    }
}
