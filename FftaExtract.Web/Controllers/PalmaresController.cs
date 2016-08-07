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

    using Ninject.Extensions.Logging;

    public class PalmaresController : JobController
    {
        private readonly PalmaresProvider palmares;
        private readonly IRepositoryImporter repository;

        private readonly ILogger logger;

        public PalmaresController(PalmaresProvider palmares, IRepositoryImporter repository, ILogger logger)
        {
            this.palmares = palmares;
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<IHttpActionResult> Get(string code)
        {
            return await this.Job(
                async () =>
                    {
                        this.logger.Info("Get palmares of {0}", code);
                        await this.InternalGet(code, null, null, null, null);
                    });
        }

        public async Task<IHttpActionResult> Get(string code, int? year)
        {
            return await this.Job(
                async () =>
                    {
                        await this.InternalGet(code, year, null, null, null);
                    });
        }

        public async Task<IHttpActionResult> Get(string code, int year, Category category, CompetitionType competitionType, BowType bowType)
        {
            return await this.Job(
                async () =>
                    {
                        await this.InternalGet(code, year, category, competitionType, bowType);
                    });
        }

        private async Task InternalGet(string code, int? year, Category? category, CompetitionType? competitionType, BowType? bowType)
        {
            var archer = this.repository.GetArcher(code);

            await this.palmares.UpdateArcher(archer, year, category, competitionType, bowType);

            this.repository.SaveArcher(archer);
        }
    }
}
