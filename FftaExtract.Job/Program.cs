using System;
using System.Linq;
using System.Threading.Tasks;

namespace FftaExtract.Job
{
    using System.Data.Entity;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Migrations;

    using log4net.Config;

    using Ninject;

    using Pattern.Config;
    using Pattern.Core.Ninject;

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

            var extractor = kernel.Get<Extractor>();
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
