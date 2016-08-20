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

    public class CompetitionController : JobController
    {
        private readonly IRepository competitionCategorieRepository;

        private readonly Job job;

        public CompetitionController(Job job, IRepository competitionCategorieRepository)
        {
            this.job = job;
            this.competitionCategorieRepository = competitionCategorieRepository;
        }

        [Route("api/competition")]
        public async Task<IHttpActionResult> Get()
        {
            return await this.Job(
                () =>
                {
                    foreach (var competitionInfo in this.competitionCategorieRepository.GetCompetitionWithoutLocation())
                    {
                        this.job.Push($"api/competition/{competitionInfo.Id}");
                    }
                });
        }

        [Route("api/competition/{id}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            return await this.Job(
                       () =>
                           {
                               var competitionInfo = this.competitionCategorieRepository.GetCompetitionInfo(id);

                               if (competitionInfo.Location == null)
                               {
                                   
                               }
                           });
        }
    }
}
