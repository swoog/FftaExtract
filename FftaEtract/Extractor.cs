namespace FftaExtract
{
    using System.Threading.Tasks;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using Ninject.Extensions.Logging;

    public class Extractor
    {
        private IStatsProvider[] providers;

        private IRepositoryImporter repositoryImporter;

        private ILogger logger;

        public Extractor(IStatsProvider[] providers, IRepositoryImporter repositoryImporter, ILogger logger)
        {
            this.providers = providers;
            this.repositoryImporter = repositoryImporter;
            this.logger = logger;
        }

        public void Run()
        {
            foreach (var provider in this.providers)
            {
                this.logger.Info("Run provider {0}", provider.GetType().Name);
                foreach (var archer in provider.GetArchers())
                {
                    this.logger.Info("Archer : {0} {1}", archer.FirstName, archer.LastName);
                    this.repositoryImporter.SaveArcher(archer);
                }

                this.logger.Info("Run provider {0} ending", provider.GetType().Name);
            }
        }
    }
}