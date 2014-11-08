using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FftaExtract.Web.Controllers
{
    using FftaExtract.DatabaseModel;
    using FftaExtract.Web.Models;

    public class ClubController : Controller
    {
        private IRepository repository;

        public ClubController(IRepository repository)
        {
            this.repository = repository;
        }

        // GET: Club
        public ActionResult Index(int id)
        {
            var club = this.repository.GetClub(id);

            var archersByYear = this.repository.GetArchersByYear(id);

            var yearPodium = this.repository.GetClubStats(id);

            var yearsModel = (from arches in archersByYear
                              join podium in yearPodium on arches.Year equals podium.Year
                              orderby arches.Year descending 
                              select new YearModel { Year = arches.Year, Archers = arches, Stats = podium, }).ToList();

            return View(new ClubModel { Club = club, Years = yearsModel, });
        }
    }
}