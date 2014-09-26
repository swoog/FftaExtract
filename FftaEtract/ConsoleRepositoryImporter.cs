namespace FftaExtract
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    public class ConsoleRepositoryImporter : IRepositoryImporter
    {
        public void SaveArcher(ArcherDataProvider archerDataProvider)
        {
            Console.WriteLine("First Name : {0}, Last Name : {1}", archerDataProvider.FirstName, archerDataProvider.LastName);

            foreach (var competition in archerDataProvider.Competitions)
            {
                Console.WriteLine("\t {0} {1} {2} : {3}", competition.Year, competition.CompetitionType, competition.Name, competition.Score);
            }
        }

        public IEnumerable<ArcherDataProvider> GetAllArchers()
        {
            yield return new ArcherDataProvider() { Code = "661811K", };
            yield return new ArcherDataProvider() { Code = "359095W", };
        }
    }
}