namespace FftaEtract
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IRepository
    {
        void SaveArcher(Archer archer);

        IEnumerable<Archer> GetAllArchers();
    }
}