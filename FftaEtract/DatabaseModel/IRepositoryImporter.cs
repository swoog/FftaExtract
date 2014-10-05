namespace FftaExtract.DatabaseModel
{
    using System.Collections.Generic;

    using FftaExtract.Providers;

    public interface IRepositoryImporter
    {
        void SaveArcher(ArcherDataProvider archerDataProvider);

        IEnumerable<ArcherDataProvider> GetAllArchers();

        ArcherDataProvider GetArcher(string code);
    }
}