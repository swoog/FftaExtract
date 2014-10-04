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

        public ArcherController(IRepository repository)
        {
            this.repository = repository;
        }

        // GET: Archer
        public ActionResult Index(string code)
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
                                    HighScores = s2.OrderByDescending(s3 => s3.Score).Take(3).ToArray(),
                                    Average = (int)Math.Ceiling((double)(s2.OrderByDescending(s3 => s3.Score).Take(3).Sum(s3 => s3.Score) / 3)),
                                    Types = (from t in s2
                                             group t by t.Competition.Type
                                                        into t2
                                             orderby t2.Key
                                             select
                                                 new TyepCompetitionModel
                                             {
                                                 Type = t2.Key,
                                                 Competitions =
                                                             t2.ToList(),
                                             }).ToList(),
                                };

            var club = this.repository.GetCurrentClub(code);

            return View(new ArcherModel
            {
                Archer = archer,
                Bows = bows,
                BestScores = bestScores,
                Competitions = qCompetttions.ToList(),
                Club = club,
            });
        }
    }
}