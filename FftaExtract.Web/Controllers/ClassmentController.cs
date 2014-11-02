namespace FftaExtract.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    public class ClassmentController : ApiController
    {
        private readonly ClassmentProvider classement;
        private readonly IRepositoryImporter repository;

        public ClassmentController(ClassmentProvider classement, IRepositoryImporter repository)
        {
            this.classement = classement;
            this.repository = repository;
        }

        // POST: api/Competion
        public async Task Post(int year, Category category, CompetitionType competitionType, BowType bowType, int page)
        {
            foreach (var archerDataProvider in await this.classement.GetArchers(year, category, competitionType, bowType, page))
            {
                this.repository.SaveArcher(archerDataProvider);
            }
        }
    }
}
