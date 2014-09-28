namespace FftaExtract.Providers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStatsProvider
    {
        IEnumerable<ArcherDataProvider> GetArchers();
    }
}