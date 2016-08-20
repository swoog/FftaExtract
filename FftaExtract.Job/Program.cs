namespace FftaExtract.Job
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FftaExtract.DatabaseModel;

    using log4net.Config;

    using Ninject;

    using Pattern.Config;
    using Pattern.Core.Ninject;
    using Pattern.Core.Interfaces;
    using Pattern.Logging.Log4net;

    // To learn more about Microsoft Azure WebJobs, please see http://go.microsoft.com/fwlink/?LinkID=401557
    class Program
    {
        static void Main()
        {
            XmlConfigurator.Configure();

            var kernel = new StandardKernel();

            var pattern = kernel.BindPattern();

            pattern.Bind<IRepository>().To<DatabaseRepository>();
            pattern.Bind<IRepositoryImporter>().To<DataBaseRepositoryImporter>();
            pattern.BindLog4net();

            var extractor = pattern.Get<Extractor>();
            try
            {
                Task.WaitAll(Task.Run(() => extractor.Run()));
            }
            catch (AggregateException ex)
            {
                throw ex.InnerExceptions.First();
            }
        }
    }
}
