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
        public async Task Post(Category category, CompetitionType competitionType, int page)
        {
            foreach (var archerDataProvider in await this.classement.GetArchers(category, competitionType, page))
            {
                this.repository.SaveArcher(archerDataProvider);
            }
        }
    }
}
