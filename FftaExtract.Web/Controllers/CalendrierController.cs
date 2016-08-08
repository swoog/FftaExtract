namespace FftaExtract.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using Ninject.Extensions.Logging;

    public class CalendrierController : JobController
    {
        private readonly CalendrierProvider calendrierProvider;
        private readonly IRepositoryImporter repository;

        private readonly ILogger logger;

        private readonly CompetitionCategorieRepository competitionCategorieRepository;

        private readonly Job job;

        public CalendrierController(CalendrierProvider calendrierProvider, IRepositoryImporter repository, ILogger logger, CompetitionCategorieRepository competitionCategorieRepository, Job job)
        {
            this.calendrierProvider = calendrierProvider;
            this.repository = repository;
            this.logger = logger;
            this.competitionCategorieRepository = competitionCategorieRepository;
            this.job = job;
        }

        [Route("api/calendrier")]
        public async Task<IHttpActionResult> Get()
        {
            return await this.Job(
                () =>
                    {
                        foreach (var category in this.competitionCategorieRepository.GetCompetitionTypes())
                        {
                            this.job.Push(
                                "api/calendrier/{0}",
                                category.EnumType);

                        }
                    });
        }

        [Route("api/calendrier/{competitionType}")]
        public async Task<IHttpActionResult> Get(CompetitionType competitionType)
        {
            var beginDate = DateTime.Now.Date;
            var endDate = beginDate.AddMonths(6);
            return await this.Job(async () =>
                    {
                        await this.calendrierProvider.Import(competitionType, beginDate, endDate);
                    });
        }
    }
}
