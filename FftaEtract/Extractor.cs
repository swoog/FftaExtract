﻿namespace FftaExtract
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using FftaExtract.Providers;

    using Newtonsoft.Json;

    using Pattern.Logging;

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

        private readonly WebJobRunning azure;

        public Extractor(Job job, ILogger logger, CompetitionCategorieRepository competitionCategorieRepository, WebJobRunning azure)
        {
            this.job = job;
            this.logger = logger;
            this.competitionCategorieRepository = competitionCategorieRepository;
            this.azure = azure;
        }

        public async Task Run()
        {
            this.logger.Info("Start extracting");

            //this.DisplayAllCategories();

            var random = new Random();

            while (this.azure.IsRunning)
            {
                var job = this.job.GetNextJobInfo();

                if (job == null)
                {
                    this.logger.Info("No job wait for a job.");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    continue;
                }

                var client = new HttpClient();
                client.Timeout = TimeSpan.FromHours(1);
             
                var uri = new Uri(new Uri(this.urlLocalHost), job.Url);

                this.logger.Info($"Start {uri}");
                var value = await client.GetStringAsync(uri);
                var response = JsonConvert.DeserializeObject<JobResult>(value);

                if (response.Error)
                {
                    this.logger.Error($"Error job : {response.ErrorMessage}");
                    this.job.Error(job, response.ErrorMessage);
                }
                else
                {
                    this.logger.Info($"Complete {uri}");
                    this.job.Complete(job);
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(random.Next(1, 200)));
            }

            this.logger.Info("Stop extractor");
        }

        private void DisplayAllCategories()
        {
            try
            {
                foreach (var category in this.competitionCategorieRepository.GetCategories(null, null))
                {
                    this.logger.Info($"Category : {category.Year} {category.CompetitionType} {category.Category} {category.BowType}");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Error to get categories");
                throw;
            }
        }
    }
}