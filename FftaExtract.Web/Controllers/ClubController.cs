using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FftaExtract.Web.Controllers
{
    using FftaExtract.DatabaseModel;

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

            return View(club);
        }
    }
}