namespace FftaExtract.DatabaseModel
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices.ComTypes;

    public class DatabaseRepository : IRepository
    {
        public Archer GetArcher(string code)
        {
            using (var db = new FftaDatabase())
            {
                return (from a in db.Archers where a.Code == code select a).FirstOrDefault();
            }
        }

        public List<BowType> GetBows(string archerCode)
        {
            using (var db = new FftaDatabase())
            {
                return (from a in db.CompetitionsScores where a.ArcherCode == archerCode select a.BowType).Distinct().ToList();
            }
        }

        public List<BestScore> GetBestScores(string code)
        {
            using (var db = new FftaDatabase())
            {
                var q = from s in db.CompetitionsScores
                        where s.ArcherCode == code
                        group s by new { s.BowType, s.Competition.Type }
                        into s2
                        select
                            new BestScore()
                        {
                            BowType = s2.FirstOrDefault().BowType,
                            CompetitionType = s2.FirstOrDefault().Competition.Type,
                            Score = s2.Max(p => p.Score),
                            CompetitionName = s2.FirstOrDefault(s => s.Score == s2.Max(p => p.Score)).Competition.CompetitionInfo.Name,
                        };

                return q.ToList();
            }
        }

        public List<CompetitionScore> GetCompetitions(string code)
        {
            using (var db = new FftaDatabase())
            {
                var q = from s in db.CompetitionsScores.Include("Competition.CompetitionInfo") where s.ArcherCode == code select s;

                return q.ToList();
            }
        }

        public List<Archer> Search(string query)
        {
            using (var db = new FftaDatabase())
            {
                var q = from a in db.Archers where a.FirstName.Contains(query) || a.LastName.Contains(query) select a;

                return q.ToList();
            }
        }

        public Club GetCurrentClub(string code)
        {
            using (var db = new FftaDatabase())
            {
                var q = from ac in db.ArchersClubs where ac.ArcherCode == code orderby ac.Year descending select ac.Club;

                return q.FirstOrDefault();
            }
        }

        public Club GetClub(int id)
        {
            using (var db = new FftaDatabase())
            {
                var q = from club in db.Clubs where club.Id == id select club;

                return q.FirstOrDefault();
            }
        }
    }
}