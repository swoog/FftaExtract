namespace FftaExtract.Providers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using FftaExtract.DatabaseModel;

    using HtmlAgilityPack;

    public class CompetitionCategoryMapping
    {
        public static Dictionary<string, int> code = new Dictionary<string, int>();

        private static HashSet<int> years = new HashSet<int>();

        public static void LoadYear(int year)
        {
            if (!years.Contains(year))
            {
                lock (years)
                {
                    if (!years.Contains(year))
                    {
                        Task.Run(() => InternalLoadYear(year)).Wait();
                    }
                }
            }
        }

        private static async Task InternalLoadYear(int year)
        {
            var client = new HttpClient();
            var content =
                new StringContent(
                    $"operation=search&urlretour=&search%5BAnnee%5D={year}&search%5BType%5D=all&search%5BSexe%5D=all&search%5BDiscipline%5D=all&search%5BArme%5D=all&search%5BCatage%5D=all",
                    Encoding.UTF8,
                    "application/x-www-form-urlencoded");
            var result = await client.PostAsync("http://classements.ffta.fr/iframe/classements.html", content);

            HtmlDocument doc = new HtmlDocument();
            doc.Load(await result.Content.ReadAsStreamAsync(), Encoding.UTF8);

            var tr = doc.DocumentNode.SelectNodes("//table[@class='crh']/tbody/tr");

            foreach (HtmlNode line in tr)
            {
                var td = line.SelectNodes("td");

                if (td[7].InnerText == "par équipe")
                {
                    continue;
                }

                var aNode = td[0].SelectNodes("a").First();
                var aHref = aNode.Attributes["href"].Value;

                var m = Regex.Match(aHref, @"/([0-9]+)\.html$");

                string val;
                if (m.Success)
                {
                    val = m.Groups[1].Value;
                }
                else
                {
                    throw new Exception($"Error parsing on {aHref}");
                }

                var a = aNode.InnerText;

                a = a.Replace($" {year}", "");

                var discipline = GetDiscipline(td[1].InnerText);
                var sex = GetSex(a);
                var categ = GetCateg(a, sex);
                var key = $"{year}_{discipline}_{categ}";

                if (code.ContainsKey(key))
                {
                    if (code[key] != Convert.ToInt32(val))
                    {
                        continue;
                        throw new NotSupportedException(key);
                    }
                }

                code.Add(key, Convert.ToInt32(val));
            }
        }

        private static string GetCateg(string name, string sex)
        {
            if (sex == "H")
            {
                name = name.Replace("Homme ", "");
            }
            else
            {
                name = name.Replace("Femme ", "");
            }

            var bow = string.Empty;

            if (name.EndsWith("Arc classique"))
            {
                bow = "CL";
                name = name.Replace(" Arc classique", "");
            }
            else if (name.EndsWith("Arc Classique"))
            {
                bow = "CL";
                name = name.Replace(" Arc Classique", "");
            }
            else if (name.EndsWith("Classique"))
            {
                bow = "CL";
                name = name.Replace(" Classique", "");
            }
            else if (name.EndsWith("Arc à Poulies"))
            {
                bow = "CO";
                name = name.Replace(" Arc à Poulies", "");
            }
            else if (name.EndsWith("Arc à poulies"))
            {
                bow = "CO";
                name = name.Replace("Arc à poulies", "");
            }
            else if (name.EndsWith("Arc a poulies"))
            {
                bow = "CO";
                name = name.Replace("Arc a poulies", "");
            }
            else if (name.EndsWith("Poulie"))
            {
                bow = "CO";
                name = name.Replace(" Poulie", "");
            }
            else if (name.EndsWith("Poulies"))
            {
                bow = "CO";
                name = name.Replace(" Poulies", "");
            }
            else if (name.EndsWith("Bare Bow"))
            {
                bow = "BB";
                name = name.Replace(" Bare Bow", "");
            }
            else if (name.EndsWith("Bare bow"))
            {
                bow = "BB";
                name = name.Replace(" Bare bow", "");
            }
            else if (name.EndsWith("Arc Droit"))
            {
                bow = "AD";
                name = name.Replace(" Arc Droit", "");
            }
            else if (name.EndsWith("Arc droit"))
            {
                bow = "AD";
                name = name.Replace(" Arc droit", "");
            }
            else if (name.EndsWith("Arc chasse"))
            {
                bow = "AC";
                name = name.Replace(" Arc chasse", "");
            }
            else if (name.EndsWith("Arc Chasse"))
            {
                bow = "AC";
                name = name.Replace(" Arc Chasse", "");
            }
            else if (name.EndsWith("Arc libre"))
            {
                bow = "AL";
                name = name.Replace(" Arc libre", "");
            }
            else if (name.EndsWith("Arc Libre"))
            {
                bow = "AL";
                name = name.Replace(" Arc Libre", "");
            }
            else if (name.EndsWith("Arc à Poulies nu"))
            {
                bow = "PN";
                name = name.Replace(" Arc à Poulies nu", "");
            }
            else if (name.EndsWith("Arc à poulies nu"))
            {
                bow = "PN";
                name = name.Replace(" Arc à poulies nu", "");
            }

            switch (name.Trim())
            {
                case "Benjamin":
                    return $"B{sex}_{bow}";
                case "Benjamine":
                    return $"B{sex}_{bow}";
                case "Minime":
                    return $"M{sex}_{bow}";
                case "Cadet":
                    return $"C{sex}_{bow}";
                case "Junior":
                    return $"J{sex}_{bow}";
                case "Senior":
                    return $"S{sex}_{bow}";
                case "Vétéran":
                    return $"V{sex}_{bow}";
                case "Veteran":
                    return $"V{sex}_{bow}";
                case "Super Vétéran":
                    return $"SV{sex}_{bow}";
                case "Super Veteran":
                    return $"SV{sex}_{bow}";
                case "Scratch":
                    return $"SC{sex}_{bow}";
                case "Scartch":
                    return $"SC{sex}_{bow}";
                case "Jeune":
                    return $"JE{sex}_{bow}";
            }

            throw new NotSupportedException(name);
        }

        private static string GetSex(string name)
        {
            if (name.Contains("Homme"))
            {
                return "H";
            }
            else if (name.Contains("Femme"))
            {
                return "F";
            }

            throw new NotSupportedException(name);
        }

        private static string GetDiscipline(string discipline)
        {
            switch (discipline.Trim())
            {
                case "Tir Fédérale":
                    return "Federal";
                case "Tir Fédéral":
                    return "Federal";
                case "Tir en Campagne":
                    return "Campagne";
                case "Tir en Salle":
                    return "Salle";
                case "Tir 3D":
                    return "3D";
                case "Tir Fita":
                    return "Fita";
                case "Tir Nature":
                    return "Nature";
            }

            throw new NotSupportedException(discipline);
        }

        static CompetitionCategoryMapping()
        {
            return;
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