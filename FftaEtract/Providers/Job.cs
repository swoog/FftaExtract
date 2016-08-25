namespace FftaExtract.Providers
{
    using System.Collections.Generic;

    using FftaExtract.DatabaseModel;

    using Pattern.Logging;

    public class Job
    {
        private readonly IRepository repository;

        private readonly ILogger logger;

        public Job(IRepository repository, ILogger logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public void Push(string api, params object[] parameters)
        {
            var url = string.Format(api, parameters);

            this.logger.Info($"Push : {url}");

            this.repository.AddJobInfo(new JobInfo() { Url = url });
        }

        public List<JobInfo> GetNextJobInfo(int take)
        {
            return this.repository.GetNextJobInfo(take);
        }

        public void Complete(JobInfo job)
        {
            this.repository.CompleteJobInfo(job);
        }

        public void Error(JobInfo job, string reasonPhrase)
        {
            this.repository.ErrorJobInfo(job, reasonPhrase);
        }
    }
}