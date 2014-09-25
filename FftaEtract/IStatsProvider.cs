namespace FftaEtract
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStatsProvider
    {
        Task<IList<Archer>> GetArchers();
    }
}