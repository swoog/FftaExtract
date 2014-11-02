using System.Linq;
using System.Text;

namespace FftaExtract
{
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

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

            var extractor = kernel.Get<Extractor>();

            Task.WaitAll(extractor.Run());

            Console.ReadLine();
        }
    }
}
