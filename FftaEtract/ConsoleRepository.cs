namespace FftaEtract
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ConsoleRepository : IRepository
    {
        public void SaveArcher(Archer archer)
        {
            Console.WriteLine("First Name : {0}, Last Name : {1}", archer.FirstName, archer.LastName);

            foreach (var competition in archer.Competitions)
            {
                Console.WriteLine("\t {0} : {1}", competition.Name, competition.Score);
            }
        }

        public IEnumerable<Archer> GetAllArchers()
        {
            yield return new Archer() { Code = "661811K", };
            yield return new Archer() { Code = "359095W", };
        }
    }
}