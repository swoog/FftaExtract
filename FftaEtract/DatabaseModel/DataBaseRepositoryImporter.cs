namespace FftaExtract.DatabaseModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;

    using FftaExtract.Providers;

    public class DataBaseRepositoryImporter : IRepositoryImporter
    {
        public void SaveArcher(ArcherDataProvider archerDataProvider)
        {
            using (var db = new FftaDatabase())
            {
                this.SaveArcherInfo(db, archerDataProvider);

                // Clean Competition score
                CleanCompetitionScore(db, archerDataProvider.Competitions);

                foreach (var competitionDataProvider in archerDataProvider.Competitions)
                {
                    this.SaveCompetitionInfo(db, archerDataProvider.Code, competitionDataProvider);
                }

                db.SaveChanges();
            }
        }

        private void CleanCompetitionScore(FftaDatabase db, IList<CompetitionDataProvider> competitions)
        {
            var q = from c in competitions group c by new { c.Begin, c.Name } into c2 select c2;

            foreach (var c in q)
            {
                this.CleanCompetitionScore(db, c.Key.Begin, c.Key.Name);
            }
        }

        private void CleanCompetitionScore(FftaDatabase db, DateTime competitions, string name)
        {
            var q = from s in db.CompetitionsScores
                    where s.Competition.CompetitionInfo.Name == name && s.Competition.Begin == competitions
                    select s;

            db.CompetitionsScores.RemoveRange(q);
            db.SaveChanges();
        }

        private void SaveCompetitionInfo(FftaDatabase db, string code, CompetitionDataProvider competitionDataProvider)
        {
            var competitionInfoId = this.SaveCompetitionInfo(db, competitionDataProvider);

            var competitionId = SaveCompetition(db, competitionDataProvider, competitionInfoId);

            this.SaveScore(db, code, competitionId, competitionDataProvider);
        }

        private void SaveScore(
            FftaDatabase db,
            string code,
            int competitionId,
            CompetitionDataProvider competitionDataProvider)
        {
            //var q = from s in db.CompetitionsScores
            //        where
            //            s.ArcherCode == code && s.Competition.Year == competitionDataProvider.Year
            //            && s.CompetitionId == competitionId
            //            && s.Competition.Type == competitionDataProvider.CompetitionType
            //        select s;

            //var score = q.FirstOrDefault();

            //if (score == null)
            //{
            db.CompetitionsScores.Add(new CompetitionScore()
            {
                ArcherCode = code,
                CompetitionId = competitionId,
                BowType = competitionDataProvider.BowType,
                Score = competitionDataProvider.Score,
                Rank = competitionDataProvider.Rank
            });

            db.SaveChanges();
            //}
        }

        private static int SaveCompetition(
            FftaDatabase db,
            CompetitionDataProvider competitionDataProvider,
            int competitionInfoId)
        {
            var q = from c in db.Competitions
                    where
                        c.Year == competitionDataProvider.Year && c.CompetitionInfoId == competitionInfoId
                        && c.Begin == competitionDataProvider.Begin && c.End == competitionDataProvider.End
                        && c.Type == competitionDataProvider.CompetitionType
                    select c;

            var competition = q.FirstOrDefault();

            if (competition == null)
            {
                competition = db.Competitions.Add(
                    new Competition()
                {
                    CompetitionInfoId = competitionInfoId,
                    Type = competitionDataProvider.CompetitionType,
                    Year = competitionDataProvider.Year,
                    Begin = competitionDataProvider.Begin,
                    End = competitionDataProvider.End,
                });
                db.SaveChanges();
            }

            return competition.Id;
        }

        private int SaveCompetitionInfo(FftaDatabase db, CompetitionDataProvider competitionDataProvider)
        {
            var q = from c in db.CompetitionInfos where c.Name == competitionDataProvider.Name select c;

            var club = q.FirstOrDefault();

            if (club == null)
            {
                club = db.CompetitionInfos.Add(new CompetitionInfo() { Name = competitionDataProvider.Name });
                db.SaveChanges();
            }

            return club.Id;
        }

        private void SaveArcherInfo(FftaDatabase db, ArcherDataProvider archerDataProvider)
        {


            var q = from a in db.Archers where a.Code == archerDataProvider.Code select a;

            var dataBaseArcher = q.FirstOrDefault();

            if (dataBaseArcher == null)
            {
                dataBaseArcher = db.Archers.Add(
                    new Archer
                {
                    Code = archerDataProvider.Code,
                    FirstName = archerDataProvider.FirstName,
                    LastName = archerDataProvider.LastName,
                    Sexe = archerDataProvider.Sexe,
                    LastUpdate = DateTime.Now
                });
            }
            else
            {
                if (dataBaseArcher.LastName == null || dataBaseArcher.FirstName == null)
                {
                    dataBaseArcher.LastName = archerDataProvider.LastName;
                    dataBaseArcher.FirstName = archerDataProvider.FirstName;
                }

                dataBaseArcher.Sexe = archerDataProvider.Sexe;
                dataBaseArcher.LastUpdate = DateTime.Now;
            }

            db.SaveChanges();

            if (archerDataProvider.Club != null)
            {
                foreach (var clubdata in archerDataProvider.Club)
                {
                    var club = this.SaveClub(db, clubdata);

                    SaveArcherClub(db, clubdata.Year, dataBaseArcher, club);
                }
            }
        }

        private void SaveArcherClub(FftaDatabase db, int year, Archer dataBaseArcher, Club club)
        {
            var q = from ac in db.ArchersClubs
                    where ac.ArcherCode == dataBaseArcher.Code && ac.ClubId == club.Id && ac.Year == year
                    select ac;

            var archerClub = q.FirstOrDefault();

            if (archerClub == null)
            {
                db.ArchersClubs.Add(
                    new ArcherClub() { ClubId = club.Id, ArcherCode = dataBaseArcher.Code, Year = year });

                db.SaveChanges();
            }
        }

        private Club SaveClub(FftaDatabase db, ClubDataProvider clubDataProvider)
        {
            var q = from c in db.Clubs where c.Name == clubDataProvider.Club select c;

            var club = q.FirstOrDefault();

            if (club == null)
            {
                club = new Club { Name = clubDataProvider.Club, };

                db.Clubs.Add(club);
                db.SaveChanges();
            }

            return club;
        }

        public IEnumerable<ArcherDataProvider> GetAllArchers()
        {
            using (var db = new FftaDatabase())
            {
                var q = from a in db.Archers
                        //where a.Code == "359095"
                        orderby a.LastUpdate
                        select a;

                foreach (var archer in q)
                {
                    yield return new ArcherDataProvider()
                    {
                        Code = archer.Code,
                        FirstName = archer.FirstName,
                        LastName = archer.LastName,
                        Sexe = archer.Sexe,
                    };
                }
            }
        }

        public ArcherDataProvider GetArcher(string code)
        {
            using (var db = new FftaDatabase())
            {
                var q = from a in db.Archers
                        where a.Code == code
                        select a;

                var archer = q.FirstOrDefault();

                if (archer == null)
                {
                    return new ArcherDataProvider() { Code = code };
                }

                return new ArcherDataProvider()
                {
                    Code = archer.Code,
                    FirstName = archer.FirstName,
                    LastName = archer.LastName,
                    Sexe = archer.Sexe,
                };
            }
        }
    }
}
