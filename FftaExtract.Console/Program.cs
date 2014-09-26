using System.Linq;
using System.Text;

namespace FftaExtract
{
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using Ninject;
    using Ninject.Activation;
    using Ninject.Selection.Heuristics;

    class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel();

            //kernel.Bind<IRepositoryImporter>().To<ConsoleRepositoryImporter>();
            kernel.Bind<IRepositoryImporter>().To<DataBaseRepositoryImporter>();

            kernel.Bind<IStatsProvider>().To<PalmaresProvider>();

            var extractor = kernel.Get<Extractor>();

            extractor.Run();

            Console.ReadLine();
        }
    }
}
