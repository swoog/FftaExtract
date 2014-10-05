using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FftaExtract.Web.Controllers
{
    using System.Security.Cryptography;

    using FftaExtract;
    using FftaExtract.DatabaseModel;
    using FftaExtract.Web.Models;

    public class ArcherController : Controller
    {
        private IRepository repository;

        private Extractor extractor;

        public ArcherController(IRepository repository, Extractor extractor)
        {
            this.repository = repository;
            this.extractor = extractor;
        }

        // GET: Archer
        public ActionResult Index(string code)
        {
            var archerModel = this.GetArcherModel(code);
            return View(archerModel);
        }

        private ArcherModel GetArcherModel(string code)
        {
            var archer = this.repository.GetArcher(code);

            var bows = this.repository.GetBows(code);

            var bestScores = this.repository.GetBestScores(code);

            var competitions = this.repository.GetCompetitions(code);

            var qCompetttions = from s in competitions
                                group s by s.Competition.Year
                                into s2
                                orderby s2.Key descending
                                select new YearCompetitionModel()
                                           {
                                               Year = s2.Key,
                                               Types = (from t in s2
                                                        group t by t.Competition.Type
                                                        into t2
                                                        orderby t2.Key
                                                        select
                                                            new TyepCompetitionModel
                                                                {
                                                                    Type = t2.Key,
                                                                    HighScores =
                                                                        t2
                                                                        .OrderByDescending
                                                                        (s3 => s3.Score)
                                                                        .Take(3)
                                                                        .ToArray(),
                                                                    Average =
                                                                        (int)
                                                                        Math.Ceiling(
                                                                            (double)
                                                                            (t2
                                                                                 .OrderByDescending
                                                                                 (
                                                                                     s3
                                                                                     =>
                                                                                     s3
                                                                                         .Score)
                                                                                 .Take(
                                                                                     3)
                                                                                 .Sum(
                                                                                     s3
                                                                                     =>
                                                                                     s3
                                                                                         .Score)
                                                                             / 3)),
                                                                    Competitions =
                                                                        t2.ToList(),
                                                                }).ToList(),
                                           };

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

        public ActionResult Update(string code)
        {
            var archer = this.repository.GetArcher(code);

            if (archer.LastUpdate < DateTime.Now.AddDays(-7))
            {
                this.extractor.UpdateArcher(code);
            }

            var archerModel = this.GetArcherModel(code);
            return this.Json(archerModel);
        }
    }
}