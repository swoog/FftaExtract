namespace FftaExtract.Providers
{
    using FftaExtract.DatabaseModel;

    public class Job
    {
        private IRepository repository;

        public Job(IRepository repository)
        {
            this.repository = repository;
        }

        public void Push(string api, params  object[] parameters)
        {
            var url = string.Format(api, parameters);

            this.repository.AddJobInfo(new JobInfo() { Url = url });
        }

        public JobInfo GetNextJobInfo()
        {
            return this.repository.GetNextJobInfo();
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