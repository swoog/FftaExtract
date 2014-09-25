namespace FftaEtract
{
    public class Competition
    {
        public string Name { get; set; }

        public int Score { get; set; }

        public CompetitionType CompetitionType { get; set; }
    }

    public enum CompetitionType
    {
        Salle,
        Fita,
        Federal,
    }
}