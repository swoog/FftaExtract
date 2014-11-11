namespace FftaExtract.Providers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class CompetitionCategoryMapping
    {
        public static Dictionary<string, int> code = new Dictionary<string, int>();

        static CompetitionCategoryMapping()
        {
            var lines = File.ReadAllLines("categories.csv").Skip(1);

            foreach (var lineT in lines)
            {
                var line = lineT.Split(';');

                if (line.Length != 7)
                {
                    continue;
                }

                var key = string.Format("{0}_{1}_{2}{3}_{4}", line[1], line[2], line[3], line[4], line[5]);

                if (code.ContainsKey(key))
                {
                    continue; // TODO : AG : Remove duplicate categorie
                    throw new NotImplementedException();
                }

                code.Add(key, Convert.ToInt32(line[6]));
            }
        }
                                                        
        public static string[] ignoredCategories = 
        {
            "(2012|2013|2014|2015)_(Salle|Fita|Federal)_JE[HF]_(CL|CO)",
            "(2012|2013|2014|2015)_Salle_J(H|F)_BB",
            "(2012|2013|2014|2015)_Salle_S(H|F)_BB",
            "(2012|2013|2014|2015)_Salle_V(H|F)_BB",
            "(2013|2014|2015)_Salle_JE(H|F)_BB", 
            "[0-9]{4}_(Fita|Federal)_[^_]+_BB", // Barebow doesn't shoot in FITA and Federal
            "(2012|2013|2014|2015)_(Salle|Federal)_SC(H|F)_(CL|CO)",
            "(2012|2013|2014|2015)_Fita_S(H|F)_(CL|CO)",
            "(2012|2013|2014|2015)_Campagne_S(H|F)_(CL|CO|BB)", // Campagne Senior are class to Scratch
            "(2012|2013|2014|2015)_Campagne_JE(H|F)_(CL|CO|BB)", // Campagne Jeune is not a categorie
        };
    }
}