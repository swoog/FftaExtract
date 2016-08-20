using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FftaExtract.Web.Controllers
{
    using System.Threading.Tasks;

    public class CompetitionController : JobController
    {
        [Route("api/competition")]
        public async Task<IHttpActionResult> Get()
        {
            return await this.Job(
                () =>
                {
                    //foreach (var category in this.competitionCategorieRepository.GetCompetitionTypes())
                    //{
                    //    this.job.Push(
                    //        "api/calendrier/{0}",
                    //        category.EnumType);

                    //}
                });
        }
    }
}
