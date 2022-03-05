using Dapper;
using DRE.Libs.SaveGame.Models;
using DRE.Libs.Setup.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
    }
}
