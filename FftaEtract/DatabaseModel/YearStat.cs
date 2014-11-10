namespace FftaExtract.DatabaseModel
{
    using System.Collections;
    using System.Collections.Generic;

    public class YearStat
    {
        public int Year { get; set; }

        public int Depart { get; set; }

        public int Podium { get; set; }

        public IList<TypeCompetitionStat> Types { get; set; }
    }
}