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
    }
}