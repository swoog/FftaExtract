using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FftaExtract.Web.Controllers
{
    using FftaExtract;
    using FftaExtract.DatabaseModel;

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


            return View(archer);
        }
    }
}