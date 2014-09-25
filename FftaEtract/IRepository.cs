namespace FftaEtract
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IRepository
    {
        void SaveArcher(ArcherDataProvider archerDataProvider);

        IEnumerable<ArcherDataProvider> GetAllArchers();
    }
}