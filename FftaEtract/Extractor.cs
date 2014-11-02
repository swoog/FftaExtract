namespace FftaExtract
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using Ninject.Extensions.Logging;

    public class Extractor
    {
        private Job job;

        private ILogger logger;

#if DEBUG
        private string urlLocalHost = "http://localhost:10151/";
#else
        private string urlLocalHost = "http://localhost/";
#endif
        public Extractor(Job job, ILogger logger)
        {
            this.job = job;
            this.logger = logger;
        }

        public async Task Run()
        {
            while (true)
            {
                var job = this.job.GetNextJobInfo();

                if (job == null)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    continue;
                }

                var client = new HttpClient();

                var uri = new Uri(new Uri(this.urlLocalHost), job.Url);

                this.logger.Info("Start {0}", uri);
                var response = await client.PostAsync(uri, null);

                if (response.IsSuccessStatusCode)
                {
                    this.job.Complete(job);
                }
                else
                {
                    this.logger.Error("Error job : {0}", response.ReasonPhrase);
                    this.job.Error(job, response.ReasonPhrase);
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}