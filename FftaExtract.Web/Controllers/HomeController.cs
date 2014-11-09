using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FftaExtract.Web.Controllers
{
    using FftaExtract.DatabaseModel;
    using FftaExtract.Web.Models;

    public class HomeController : Controller
    {
        private IRepository repository;

        public HomeController(IRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult Index()
        {
            var stats = this.repository.GetGlobalStats();

            var lastCompetitions = this.repository.GetLastCompetitions();

            return View(new HomeModel
                            {
                                Stats = stats,
                                LastCompetitions = lastCompetitions,
                            });
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Search(string query)
        {
            var archers = this.repository.Search(query);

            return this.View(archers);
        }
    }
}