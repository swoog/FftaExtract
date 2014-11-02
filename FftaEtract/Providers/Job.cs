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
    }
}