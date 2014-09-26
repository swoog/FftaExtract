﻿namespace FftaExtract.Providers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStatsProvider
    {
        Task<IList<ArcherDataProvider>> GetArchers();
    }
}