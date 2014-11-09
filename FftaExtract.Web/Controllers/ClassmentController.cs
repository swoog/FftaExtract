namespace FftaExtract.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using Ninject.Extensions.Logging;

    public class ClassmentController : ApiController
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

        public void Get(int year, int page)
        {
            foreach (var category in this.competitionCategorieRepository.GetCategories(null, year))
            {
                this.logger.Info("Push : {0} {1} {2} {3}", category.Year, category.CompetitionType, category.Category, category.BowType);

                this.job.Push("api/Classment/{0}/{1}/{2}/{3}", category.Year, category.Category, category.CompetitionType, category.BowType);
            }
        }

        public async Task Get(int year, Category category, CompetitionType competitionType, BowType bowType, int page)
        {
            this.logger.Info("Get classement of {1} {0} {2} {3} page {4}", year, category, competitionType, bowType, page);

            foreach (var archerDataProvider in await this.classement.GetArchers(year, category, competitionType, bowType, page))
            {
                this.repository.SaveArcher(archerDataProvider);
            }
        }
    }
}
