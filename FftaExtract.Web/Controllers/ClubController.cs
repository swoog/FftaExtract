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

            return View(new ClubModel
                            {
                                Club = club,
                                YearsArchers = archersByYear,
                            });
        }
    }
}