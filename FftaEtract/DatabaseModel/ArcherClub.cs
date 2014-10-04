namespace FftaExtract.DatabaseModel
{
    public class ArcherClub
    {
        public int Id { get; set; }

        public string ArcherCode { get; set; }

        public Archer Archer { get; set; }

        public int Year { get; set; }

        public int ClubId { get; set; }

        public Club Club { get; set; }
    }
}