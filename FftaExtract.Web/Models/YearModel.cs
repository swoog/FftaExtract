namespace FftaExtract.Web.Models
{
    using FftaExtract.DatabaseModel;

    public class YearModel
    {
        public YearArcher Archers { get; set; }

        public YearStat Stats { get; set; }

        public int Year { get; set; }
    }
}