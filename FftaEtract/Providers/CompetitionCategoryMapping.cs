namespace FftaExtract.Providers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class CompetitionCategoryMapping
    {
        public static Dictionary<string, int> code = new Dictionary<string, int>();

        static CompetitionCategoryMapping()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "FftaExtract.categories.csv";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();

                var lines = result.Split('\n').Skip(1);

                foreach (var lineT in lines)
                {
                    var line = lineT.Replace("\r", "").Split(';');

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
        }

        public static string[] ignoredCategories = 
        {
            "(2011|2012|2013|2014|2015)_(Salle|Fita|Federal)_JE[HF]_(CL|CO)",
            "(2011|2012|2013|2014|2015)_Salle_J(H|F)_BB",
            "2011_Salle_SC(H|F)_BB", // Barebow are senior in 2011 and scratch after 2011
            "(2012|2013|2014|2015)_Salle_S(H|F)_BB", // Barebow senior are scratch after 2011
            "(2011|2012|2013|2014|2015)_Salle_V(H|F)_BB", 
            "(2011|2013|2014|2015)_Salle_JE(H|F)_BB", 
            "[0-9]{4}_(Fita|Federal)_[^_]+_BB", // Barebow doesn't shoot in FITA and Federal
            "(2011|2012|2013|2014|2015)_(Salle|Federal)_SC(H|F)_(CL|CO)",
            "(2011|2012|2013|2014|2015)_Fita_S(H|F)_(CL|CO)",
            "(2011|2012|2013|2014|2015)_Campagne_S(H|F)_(CL|CO|BB)", // Campagne Senior are class to Scratch
            "(2011|2012|2013|2014|2015)_Campagne_JE(H|F)_(CL|CO|BB)", // Campagne Jeune is not a categorie
            "(2011|2012)_Salle_C(H|F)_(CO|BB)", // No compound and barebow for Cadet before 2012
            "2011_Fita_C(H|F)_CO", // No compound for Cadet before 2011
            "(2011|2012)_Federal_C(H|F)_CO", // No compound for Cadet before 2012
            "(2011|2012|2013|2014)_Campagne_C(H|F)_CO", // No compound for Cadet in campagne
        };
    }
}