namespace FftaExtract.DatabaseModel
{
    using System.Collections.Generic;
    using System.Data.Entity;

    public class FftaDatabase : DbContext
    {
        public DbSet<Archer> Archers { get; set; }

        public DbSet<Competition> Competitions { get; set; }

        public DbSet<CompetitionInfo> CompetitionInfos { get; set; }

        public DbSet<CompetitionScore> CompetitionsScores { get; set; }

        public DbSet<Club> Clubs { get; set; }

        public DbSet<ArcherClub> ArchersClubs { get; set; }

        public DbSet<JobInfo> JobsInfos { get; set; }
    }
}
