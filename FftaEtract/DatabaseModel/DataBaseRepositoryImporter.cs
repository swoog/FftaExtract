namespace FftaExtract.DatabaseModel
{
    using System.Collections.Generic;
    using System.Linq;

    using FftaExtract.Providers;

    public class DataBaseRepositoryImporter : IRepositoryImporter
    {
        public void SaveArcher(ArcherDataProvider archerDataProvider)
        {
            using (var db = new FftaDatabase())
            {
                this.SaveArcherInfo(db, archerDataProvider);

                foreach (var competitionDataProvider in archerDataProvider.Competitions)
                {
                    this.SaveCompetitionInfo(db, archerDataProvider.Code, competitionDataProvider);
                }

                db.SaveChanges();
            }
        }

        private void SaveCompetitionInfo(FftaDatabase db, string code, CompetitionDataProvider competitionDataProvider)
        {
            var competitionInfoId = this.SaveClubInfo(db, competitionDataProvider);

            SaveCompetition(db, competitionDataProvider, competitionInfoId);

            this.SaveScore(db, competitionDataProvider);
        }

        private void SaveScore(FftaDatabase db, CompetitionDataProvider competitionDataProvider)
        {
            // TODO : AG : Save score
        }

        private static void SaveCompetition(
            FftaDatabase db,
            CompetitionDataProvider competitionDataProvider,
            int competitionInfoId)
        {
            var q = from c in db.Competitions
                    where
                        c.Year == competitionDataProvider.Year && c.CompetitionInfoId == competitionInfoId
                        && c.Type == competitionDataProvider.CompetitionType
                    select c;

            var competition = q.FirstOrDefault();

            if (competition == null)
            {
                db.Competitions.Add(
                    new Competition()
                        {
                            CompetitionInfoId = competitionInfoId,
                            Type = competitionDataProvider.CompetitionType,
                            Year = competitionDataProvider.Year,
                        });
                db.SaveChanges();
            }
        }

        private int SaveClubInfo(FftaDatabase db, CompetitionDataProvider competitionDataProvider)
        {
            var q = from c in db.Clubs where c.Name == competitionDataProvider.Name select c;

            var club = q.FirstOrDefault();

            if (club == null)
            {
                club = db.Clubs.Add(new CompetitionInfo() { Name = competitionDataProvider.Name });
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
                db.Archers.Add(
                    new Archer()
                        {
                            Code = archerDataProvider.Code,
                            FirstName = archerDataProvider.FirstName,
                            LastName = archerDataProvider.LastName,
                        });
            }
            else
            {
                dataBaseArcher.LastName = archerDataProvider.LastName;
                dataBaseArcher.FirstName = archerDataProvider.FirstName;
            }

            db.SaveChanges();
        }

        public IEnumerable<ArcherDataProvider> GetAllArchers()
        {
            yield return new ArcherDataProvider() { Code = "661811K", };
            yield return new ArcherDataProvider() { Code = "359095W", };
            yield return new ArcherDataProvider() { Code = "819573R" };
        }
    }
}
