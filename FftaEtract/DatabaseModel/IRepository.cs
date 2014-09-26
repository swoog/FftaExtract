namespace FftaExtract.DatabaseModel
{
    using System.Collections.Generic;

    public interface IRepository
    {

        Archer GetArcher(string code);

        List<string> GetBows(string archerCode);
    }
}