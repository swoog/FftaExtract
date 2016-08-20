namespace FftaExtract.DatabaseModel
{
    using System.Data.Entity.Spatial;

    public class CompetitionInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DbGeography Location { get; set; }
    }
}