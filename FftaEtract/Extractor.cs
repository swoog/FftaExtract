namespace FftaExtract
{
    using System;
    using System.Collections;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using Ninject.Extensions.Logging;

    public class Extractor
    {
        private readonly Job job;

        private readonly ILogger logger;

        private readonly CompetitionCategorieRepository competitionCategorieRepository;

#if DEBUG
        private string urlLocalHost = "http://localhost:10151/";
#else
        private string urlLocalHost = "http://fftaextract.azurewebsites.net/";
#endif
        public Extractor(Job job, ILogger logger, CompetitionCategorieRepository competitionCategorieRepository)
        {
            this.job = job;
            this.logger = logger;
            this.competitionCategorieRepository = competitionCategorieRepository;
        }

        public async Task Run()
        {
            this.logger.Info("Start extracting");

            try
            {
                foreach (var category in this.competitionCategorieRepository.GetCategories(null, null))
                {
                    this.logger.Info("Category : {0} {1} {2}", category.Year, category.CompetitionType, category.Category);
                }
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Error to get categories");
                throw;
            }

            var random = new Random();

            while (true)
            {
                var job = this.job.GetNextJobInfo();

                if (job == null)
                {
                    this.logger.Info("No job wait for a job.");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    continue;
                }

                var client = new HttpClient();

                var uri = new Uri(new Uri(this.urlLocalHost), job.Url);

                this.logger.Info("Start {0}", uri);
                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    this.job.Complete(job);
                }
                else
                {
                    this.logger.Error("Error job : {0}", response.ReasonPhrase);
                    this.job.Error(job, response.ReasonPhrase);
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(random.Next(1, 1000)));
            }
        }
    }
}