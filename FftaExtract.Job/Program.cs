﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FftaExtract.Job
{
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Model;

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

            kernel.Bind<IRepository>().To<DatabaseRepository>();
            kernel.Bind<IRepositoryImporter>().To<DataBaseRepositoryImporter>();

            var extractor = kernel.Get<Extractor>();
            try
            {
                Task.WaitAll(extractor.Run());
            }
            catch (AggregateException ex)
            {
                throw ex.InnerExceptions.First();
            }
        }
    }
}
