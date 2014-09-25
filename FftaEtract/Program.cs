using System.Linq;
using System.Text;

namespace FftaEtract
{
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    using Ninject;
    using Ninject.Activation;
    using Ninject.Selection.Heuristics;

    class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel();

            //kernel.Bind<IRepository>().To<ConsoleRepository>();
            kernel.Bind<IRepository>().To<DataBaseRepository>();

            kernel.Bind<IStatsProvider>().To<PalmaresProvider>();

            var extractor = kernel.Get<Extractor>();

            extractor.Run();

            Console.ReadLine();
        }
    }
}
