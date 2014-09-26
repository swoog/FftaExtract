using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FftaExtract.Web.Controllers
{
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


            return View(new ArcherModel { Archer = archer, Bows = bows });
        }
    }
}