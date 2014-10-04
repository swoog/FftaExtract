namespace FftaExtract.Web.Models
{
    using System.Collections.Generic;

    using FftaExtract.DatabaseModel;

    public class ClubModel 
    {
        public Club Club { get; set; }

        public List<YearArcher> YearsArchers { get; set; }
    }
}