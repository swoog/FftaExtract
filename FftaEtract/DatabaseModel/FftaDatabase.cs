namespace FftaExtract.DatabaseModel
{
    using System.Collections.Generic;
    using System.Data.Entity;

    public class FftaDatabase : DbContext
    {
        public DbSet<Archer> Archers { get; set; }

        public DbSet<Competition> Competitions { get; set; }

        public DbSet<CompetitionInfo> Clubs { get; set; }

        public DbSet<CompetitionScore> CompetitionsScores { get; set; }
    }
}
