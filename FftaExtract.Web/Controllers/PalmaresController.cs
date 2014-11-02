using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FftaExtract.Web.Controllers
{
    using System.Threading.Tasks;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    public class PalmaresController : ApiController
    {
        private readonly PalmaresProvider palmares;
        private readonly IRepositoryImporter repository;

        public PalmaresController(PalmaresProvider palmares,  IRepositoryImporter repository)
        {
            this.palmares = palmares;
            this.repository = repository;
        }

        public async Task Post(string code)
        {
            await Post(code, null);
        }

        // POST: api/Competion
        public async Task Post(string code, int? year)
        {
            var archer = this.repository.GetArcher(code);

            await this.palmares.UpdateArcher(archer, year);

            this.repository.SaveArcher(archer);
        }
    }
}
