
namespace FftaExtract.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;
    using FftaExtract.Web.Models;

    public class ArcherController : Controller
    {
        private readonly IRepository repository;

        private readonly Job job;

        public ArcherController(IRepository repository, Job job)
        {
            this.repository = repository;
            this.job = job;
        }

        // GET: Archer
        public ActionResult Index(string code)
        {
            var archerModel = this.GetArcherModel(code);

            if (archerModel == null)
            {
                return this.HttpNotFound();
            }

            return View(archerModel);
        }

        private ArcherModel GetArcherModel(string code)
        {
            var archer = this.repository.GetArcher(code);

            if (archer == null)
            {
                return null;
            }

            var bows = this.repository.GetBows(code);

            var bestScores = this.repository.GetBestScores(code);

            var competitions = this.repository.GetCompetitions(code);

            var qCompetttions = from s in competitions
                                group s by s.Competition.Year
                                into s2
                                orderby s2.Key descending
                                select CreateYearCompetitionModel(s2);

            var club = this.repository.GetCurrentClub(code);

            var archerModel = new ArcherModel
            {
                Archer = archer,
                Bows = bows,
                BestScores = bestScores,
                Competitions = qCompetttions.ToList(),
                Club = club,
            };
            return archerModel;
        }

        private static YearCompetitionModel CreateYearCompetitionModel(IGrouping<int, CompetitionScore> s2)
        {
            var tyepCompetitionModels = (from t in s2
                                         group t by new CompetitionTypeBowType(t.Competition.Type, t.BowType)
                                          into t2
                                         orderby t2.Key
                                         select
                                             CreateTyepCompetitionModel(t2)).ToList();
            return new YearCompetitionModel()
            {
                Year = s2.Key,
                Types = tyepCompetitionModels,
            };
        }

        private static TyepCompetitionModel CreateTyepCompetitionModel(IGrouping<CompetitionTypeBowType, CompetitionScore> t2)
        {
            return new TyepCompetitionModel
            {
                Info = t2.Key,
                HighScores = t2.OrderByDescending(s3 => s3.Score).Take(3).ToArray(),
                Average =
                               (int)
                               Math.Ceiling(
                                   (double)
                                   (t2.OrderByDescending(s3 => s3.Score).Take(3).Sum(s3 => s3.Score)
                                    / 3)),
                Competitions = t2.ToList(),
            };
        }

        public ActionResult Update(string code)
        {
            var archer = this.repository.GetArcher(code);

            if (archer.LastUpdate < DateTime.Now.AddDays(-7))
            {
                this.job.Push("api/Palmares/{0}", code);
            }

            var archerModel = this.GetArcherModel(code);
            return this.Json(archerModel);
        }
    }
}