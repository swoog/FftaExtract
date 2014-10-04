namespace FftaExtract.DatabaseModel
{
    using System.Collections.Generic;

    public class YearArcher
    {
        public IList<Archer> Archers { get; set; }

        public int Year { get; set; }
    }
}