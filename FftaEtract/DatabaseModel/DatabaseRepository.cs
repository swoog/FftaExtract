namespace FftaExtract.DatabaseModel
{
    using System.Collections.Generic;
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

        public List<string> GetBows(string archerCode)
        {
            using (var db = new FftaDatabase()) 
            {
                //return (from a in db.Competitions)
                return null;
            }
        }
    }
}