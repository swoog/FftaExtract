namespace FftaExtract.Providers
{
    using FftaExtract.DatabaseModel;

    public class CompetitionCategory 
    {
        public CompetitionType CompetitionType { get; private set; }

        public int IdFfta { get; private set; }

        public BowType BowType { get; private set; }

        public int Year { get; private set; }

        public Sexe Sexe { get; private set; }

        public Category Category { get; private set; }

        public CompetitionCategory(CompetitionType competitionType, BowType bowType, int idFFTA, int year, Sexe sexe, Category category)
        {
            this.Category = category;
            this.Year = year;
            this.Sexe = sexe;
            this.BowType = bowType;
            this.CompetitionType = competitionType;
            this.IdFfta = idFFTA;
        }
    }
}