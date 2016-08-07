﻿using System;
using System.Linq;
using System.Threading.Tasks;

namespace FftaExtract.Job
{
    using FftaExtract.DatabaseModel;

    using log4net.Config;

    using Ninject;

    // To learn more about Microsoft Azure WebJobs, please see http://go.microsoft.com/fwlink/?LinkID=401557
    class Program
    {
        static void Main()
        {
            XmlConfigurator.Configure();

            var kernel = new StandardKernel();

            kernel.Bind<IRepository>().To<DatabaseRepository>();
            kernel.Bind<IRepositoryImporter>().To<DataBaseRepositoryImporter>();

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
