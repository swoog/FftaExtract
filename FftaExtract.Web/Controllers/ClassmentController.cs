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

        public ClassmentController(ClassmentProvider classement, IRepositoryImporter repository, ILogger logger)
        {
            this.classement = classement;
            this.repository = repository;
            this.logger = logger;
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
