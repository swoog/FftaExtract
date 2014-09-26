namespace FftaExtract.DatabaseModel
{
    using System.Linq;
    using System.Runtime.InteropServices.ComTypes;

    public class DatabaseRepository : IRepository
    {
        public Archer GetArcher(string code)
        {
            using (var db = new FftaDatabase())
            {
                return (from a in db.Archers where a.Code == code select a).FirstOrDefault();
            }
        }
    }
}