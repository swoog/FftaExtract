namespace FftaEtract
{
    public class CompetitionCategory 
    {
        public CompetitionType CompetitionType { get; set; }

        public int IdFfta { get; set; }

        public CompetitionCategory(CompetitionType competitionType, int idFFTA)
        {
            this.CompetitionType = competitionType;
            this.IdFfta = idFFTA;
        }
    }
}