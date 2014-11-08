namespace FftaExtract.DatabaseModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices.ComTypes;
    using System.Security.Cryptography;

    using FftaExtract.Providers;

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

        public List<YearArcher> GetArchersByYear(int id)
        {
            using (var db = new FftaDatabase())
            {
                var archers = (from ac in db.ArchersClubs.Include("Archer") where ac.ClubId == id select ac).ToList();

                var q = from ac in archers
                        group ac by ac.Year into year
                        select new YearArcher()
                        {
                            Year = year.Key,
                            Archers = year.OrderBy(a => a.Archer.FullName).Select(a => a.Archer).ToArray(),
                        };

                return q.ToList();
            }
        }

        public void AddJobInfo(JobInfo jobInfo)
        {
            using (var db = new FftaDatabase())
            {
                this.CleanJobs(db);

                var q = from j in db.JobsInfos
                        where j.Url == jobInfo.Url && j.JobStatus == JobStatus.None
                        select j;

                if (!q.Any())
                {
                    jobInfo.JobStatus = JobStatus.None;
                    jobInfo.CreatedDateTime = DateTime.Now;

                    db.JobsInfos.Add(jobInfo);
                    db.SaveChanges();                    
                }
            }
        }

        public JobInfo GetNextJobInfo()
        {
            using (var db = new FftaDatabase())
            {
                this.CleanJobs(db);

                var q = from j in db.JobsInfos
                        where j.JobStatus == JobStatus.None
                        orderby j.CreatedDateTime
                        select j;

                return q.FirstOrDefault();
            }
        }

        public void CompleteJobInfo(JobInfo job)
        {
            using (var db = new FftaDatabase())
            {
                var q = from j in db.JobsInfos
                        where j.Id == job .Id
                        select j;
                var j1 = q.First();

                j1.JobStatus = JobStatus.Done;
                db.SaveChanges();
            }
        }

        public void ErrorJobInfo(JobInfo job, string reasonPhrase)
        {
            using (var db = new FftaDatabase())
            {
                var q = from j in db.JobsInfos
                        where j.Id == job.Id
                        select j;
                var j1 = q.First();

                j1.JobStatus = JobStatus.Error;
                db.SaveChanges();
            }
        }

        public IList<YearStat> GetClubStats(int clubId)
        {
            using (var db = new FftaDatabase())
            {
                var q = from ac in db.ArchersClubs
                                 join competitionScore in db.CompetitionsScores on ac.ArcherCode equals
                                     competitionScore.ArcherCode
                                 where ac.ClubId == clubId
                                 && ac.Year == competitionScore.Competition.Year
                                 group competitionScore by ac.Year
                                 into score
                                 select new { score.Key, Depart = score.Count(), Podium = score.Count(s => s.Rank <= 3) };
                var archers = q.ToList();

                return archers.Select(s => new YearStat { Year = s.Key, Depart = s.Depart, Podium = s.Podium, })
                    .ToList();
            }
        }

        public GlobalStats GetGlobalStats()
        {
            using (var db = new FftaDatabase())
            {
                var countArchers = db.Archers.Count();
                var countClubs = db.Clubs.Count();

                return new GlobalStats() { CountArchers = countArchers, CountClubs = countClubs };
            }
        }

        private void CleanJobs(FftaDatabase db)
        {
            var d = DateTime.Now.AddDays(-2);

            var jobs = from j in db.JobsInfos 
                       where j.JobStatus == JobStatus.Done 
                           && j.CreatedDateTime < d 
                       select j;

            db.JobsInfos.RemoveRange(jobs);
            db.SaveChanges();
        }
    }
}