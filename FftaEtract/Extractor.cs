namespace FftaExtract
{
    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    public class Extractor
    {
        private IStatsProvider[] providers;

        private IRepositoryImporter repositoryImporter;

        public Extractor(IStatsProvider[] providers, IRepositoryImporter repositoryImporter)
        {
            this.providers = providers;
            this.repositoryImporter = repositoryImporter;
        }

        public async void Run()
        {
            foreach (var provider in this.providers)
            {
                foreach (var archer in await provider.GetArchers())
                {
                    this.repositoryImporter.SaveArcher(archer);
                }
            }
        }
    }
}