namespace FftaExtract.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using Ninject.Extensions.Logging;

    public class ClassmentController : JobController
    {
        private readonly ClassmentProvider classement;
        private readonly IRepositoryImporter repository;

        private readonly ILogger logger;

        private readonly CompetitionCategorieRepository competitionCategorieRepository;

        private readonly Job job;

        public ClassmentController(ClassmentProvider classement, IRepositoryImporter repository, ILogger logger, CompetitionCategorieRepository competitionCategorieRepository, Job job)
        {
            this.classement = classement;
            this.repository = repository;
            this.logger = logger;
            this.competitionCategorieRepository = competitionCategorieRepository;
            this.job = job;
        }

        [Route("api/classment/{year}")]
        public async Task<IHttpActionResult> Get(int year)
        {
            return await this.Job(
                () =>
                    {
                        foreach (var category in this.competitionCategorieRepository.GetCategories(null, year))
                        {
                            this.job.Push(
                                "api/Classment/{0}/{1}/{2}/{3}",
                                category.Year,
                                category.Category,
                                category.CompetitionType,
                                category.BowType);

                        }
                    });
        }

        [Route("api/classment/{year}/{category}/{competitionType}/{bowType}")]
        public async Task<IHttpActionResult> Get(int year, Category category, CompetitionType competitionType, BowType bowType)
        {
            return await this.Job(
                async () =>
                    {
                        this.logger.Info("Get classement of {1} {0} {2} {3}", year, category, competitionType, bowType);

                        foreach (var archerDataProvider in
                            await this.classement.GetArchers(year, category, competitionType, bowType))
                        {
                            this.repository.SaveArcher(archerDataProvider);
                        }
                    });
        }
    }
}
