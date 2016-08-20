namespace FftaExtract.DatabaseModel
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IRepository
    {

        Archer GetArcher(string code);

        List<BowType> GetBows(string archerCode);

        List<BestScore> GetBestScores(string code);

        List<CompetitionScore> GetCompetitions(string code);

        List<Archer> Search(string query);

        Club GetCurrentClub(string code);

        Club GetClub(int id);

        List<YearArcher> GetArchersByYear(int id);

        void AddJobInfo(JobInfo jobInfo);

        JobInfo GetNextJobInfo();

        void CompleteJobInfo(JobInfo job);

        void ErrorJobInfo(JobInfo job, string reasonPhrase);

        IList<YearStat> GetClubStats(int clubId);

        GlobalStats GetGlobalStats();

        IList<CompetitionStats> GetLastCompetitions();

        IList<CompetitionInfo> GetCompetitionWithoutLocation();

        CompetitionInfo GetCompetitionInfo(int id);
    }
}