namespace FftaEtract
{
    using System.Collections.Generic;
    using System.Data.Entity;

    using FftaEtract.DatabaseModel;

    public class FftaDatabase : DbContext
    {
        public DbSet<Archer> Archers { get; set; }

        public DbSet<Competition> Competitions { get; set; }

        public DbSet<CompetitionInfo> Clubs { get; set; }
    }
}
