using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FftaExtract.Job
{
    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using log4net.Config;

    using Ninject;

    // To learn more about Microsoft Azure WebJobs, please see http://go.microsoft.com/fwlink/?LinkID=401557
    class Program
    {
        static void Main()
        {
            XmlConfigurator.Configure();

            var kernel = new StandardKernel();

            kernel.Bind<IRepositoryImporter>().To<DataBaseRepositoryImporter>();

            kernel.Bind<IStatsProvider>().To<PalmaresProvider>();
            kernel.Bind<IStatsProvider>().To<ClassmentProvider>();

            var extractor = kernel.Get<Extractor>();

            extractor.Run();
        }
    }
}
