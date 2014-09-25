namespace FftaEtract
{
    using System;
    using System.Collections;

    public class Extractor
    {
        private IStatsProvider[] providers;

        private IRepository repository;

        public Extractor(IStatsProvider[] providers, IRepository repository)
        {
            this.providers = providers;
            this.repository = repository;
        }

        public async void Run()
        {
            foreach (var provider in this.providers)
            {
                foreach (var archer in await provider.GetArchers())
                {
                    this.repository.SaveArcher(archer);
                }
            }
        }
    }
}