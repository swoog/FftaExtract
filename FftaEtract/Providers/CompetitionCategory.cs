namespace FftaExtract.Providers
{
    using FftaExtract.DatabaseModel;

    public class CompetitionCategory 
    {
        public CompetitionType CompetitionType { get; set; }

        public int IdFfta { get; set; }

        public BowType BowType { get; set; }

        public CompetitionCategory(CompetitionType competitionType, BowType bowType, int idFFTA)
        {
            this.BowType = bowType;
            this.CompetitionType = competitionType;
            this.IdFfta = idFFTA;
        }
    }
}