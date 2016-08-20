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
        private static Dictionary<string, int> code = new Dictionary<string, int>();

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
            var competitionCategories = await GetCompetionCategoriesForYear(year);

            CompetitionCategories.AddRange(competitionCategories);
        }

        public static async Task<List<CompetitionCategory>> GetCompetionCategoriesForYear(int year)
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

            var tr = doc.DocumentNode.SelectNodes("//table[@class='orbe3 full']/tbody/tr");

            var competitionCategories = new List<CompetitionCategory>();

            foreach (HtmlNode line in tr)
            {
                var td = line.SelectNodes("td");

                if (td[7].InnerText == "par équipe")
                {
                    continue;
                }

                var aHref = line.Attributes["data-href"].Value;

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

                var a = td[0].InnerText;

                a = a.Replace($" {year}", "");

                var competitionType = GetDiscipline(td[1].InnerText);
                var sex = GetSex(a);
                var categ = GetCateg(a, sex);
                var competitionCategory = new CompetitionCategory(
                                              competitionType,
                                              categ.Bow,
                                              Convert.ToInt32(val),
                                              year,
                                              sex,
                                              categ.Category);

                competitionCategories.Add(competitionCategory);
            }
            return competitionCategories;
        }

        private static BowTypeCategory GetCateg(string name, Sexe sex)
        {
            if (sex == Sexe.Homme)
            {
                name = name.Replace("Homme ", "");
            }
            else
            {
                name = name.Replace("Femme ", "");
            }

            BowType bow;

            if (name.EndsWith("Arc classique"))
            {
                bow = BowType.Classique;
                name = name.Replace(" Arc classique", "");
            }
            else if (name.EndsWith("Arc Classique"))
            {
                bow = BowType.Classique;
                name = name.Replace(" Arc Classique", "");
            }
            else if (name.EndsWith("Classique"))
            {
                bow = BowType.Classique;
                name = name.Replace(" Classique", "");
            }
            else if (name.EndsWith("Arc à Poulies"))
            {
                bow = BowType.Poulie;
                name = name.Replace(" Arc à Poulies", "");
            }
            else if (name.EndsWith("Arc à poulies"))
            {
                bow = BowType.Poulie;
                name = name.Replace("Arc à poulies", "");
            }
            else if (name.EndsWith("Arc a poulies"))
            {
                bow = BowType.Poulie;
                name = name.Replace("Arc a poulies", "");
            }
            else if (name.EndsWith("Poulie"))
            {
                bow = BowType.Poulie;
                name = name.Replace(" Poulie", "");
            }
            else if (name.EndsWith("Poulies"))
            {
                bow = BowType.Poulie;
                name = name.Replace(" Poulies", "");
            }
            else if (name.EndsWith("Bare Bow"))
            {
                bow = BowType.BareBow;
                name = name.Replace(" Bare Bow", "");
            }
            else if (name.EndsWith("Bare bow"))
            {
                bow = BowType.BareBow;
                name = name.Replace(" Bare bow", "");
            }
            else if (name.EndsWith("Arc Droit"))
            {
                bow = BowType.Droit;
                name = name.Replace(" Arc Droit", "");
            }
            else if (name.EndsWith("Arc droit"))
            {
                bow = BowType.Droit;
                name = name.Replace(" Arc droit", "");
            }
            else if (name.EndsWith("Arc chasse"))
            {
                bow = BowType.Chasse;
                name = name.Replace(" Arc chasse", "");
            }
            else if (name.EndsWith("Arc Chasse"))
            {
                bow = BowType.Chasse;
                name = name.Replace(" Arc Chasse", "");
            }
            else if (name.EndsWith("Arc libre"))
            {
                bow = BowType.Libre;
                name = name.Replace(" Arc libre", "");
            }
            else if (name.EndsWith("Arc Libre"))
            {
                bow = BowType.Libre;
                name = name.Replace(" Arc Libre", "");
            }
            else if (name.EndsWith("Arc à Poulies nu"))
            {
                bow = BowType.PoulieNu;
                name = name.Replace(" Arc à Poulies nu", "");
            }
            else if (name.EndsWith("Arc à poulies nu"))
            {
                bow = BowType.PoulieNu;
                name = name.Replace(" Arc à poulies nu", "");
            }
            else
            {
                throw new NotSupportedException(name);
            }

            switch (name.Trim())
            {
                case "Benjamin":
                    return new BowTypeCategory(bow, sex == Sexe.Homme ? Category.BenjaminHomme : Category.BenjaminFemme);
                case "Benjamine":
                    return new BowTypeCategory(bow, sex == Sexe.Homme ? Category.BenjaminHomme : Category.BenjaminFemme);
                case "Minime":
                    return new BowTypeCategory(bow, sex == Sexe.Homme ? Category.MinimeHomme : Category.MinimeFemme);
                case "Cadet":
                    return new BowTypeCategory(bow, sex == Sexe.Homme ? Category.CadetHomme : Category.CadetFemme);
                case "Junior":
                    return new BowTypeCategory(bow, sex == Sexe.Homme ? Category.JuniorHomme : Category.JuniorFemme);
                case "Senior":
                    return new BowTypeCategory(bow, sex == Sexe.Homme ? Category.SeniorHomme : Category.SeniorFemme);
                case "Vétéran":
                    return new BowTypeCategory(bow, sex == Sexe.Homme ? Category.VeteranHomme : Category.VeteranFemme);
                case "Veteran":
                    return new BowTypeCategory(bow, sex == Sexe.Homme ? Category.VeteranHomme : Category.VeteranFemme);
                case "Super Vétéran":
                    return new BowTypeCategory(bow, sex == Sexe.Homme ? Category.SuperVeteranHomme : Category.SuperVeteranFemme);
                case "Super Veteran":
                    return new BowTypeCategory(bow, sex == Sexe.Homme ? Category.SuperVeteranHomme : Category.SuperVeteranFemme);
                case "Scratch":
                    return new BowTypeCategory(bow, sex == Sexe.Homme ? Category.ScratchHomme : Category.ScratchFemme);
                case "Scartch":
                    return new BowTypeCategory(bow, sex == Sexe.Homme ? Category.ScratchHomme : Category.ScratchFemme);
                case "Jeune":
                    return new BowTypeCategory(bow, sex == Sexe.Homme ? Category.JeuneHomme : Category.JeuneFemme);
                case "Poussin":
                    return new BowTypeCategory(bow, sex == Sexe.Homme ? Category.PoussinHomme : Category.PoussinFemme);
            }

            throw new NotSupportedException(name);
        }

        private static Sexe GetSex(string name)
        {
            if (name.Contains("Homme"))
            {
                return Sexe.Homme;
            }
            else if (name.Contains("Femme"))
            {
                return Sexe.Femme;
            }

            throw new NotSupportedException(name);
        }

        private static CompetitionType GetDiscipline(string discipline)
        {
            switch (discipline.Trim())
            {
                case "Tir Fédérale":
                    return CompetitionType.Federal;
                case "Tir Fédéral":
                    return CompetitionType.Federal;
                case "Tir en Campagne":
                    return CompetitionType.Campagne;
                case "Tir en Salle":
                    return CompetitionType.Salle;
                case "Tir 3D":
                    return CompetitionType.Parcour3D;
                case "Tir Fita":
                    return CompetitionType.Fita;
                case "Tir Nature":
                    return CompetitionType.Nature;
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

        public static List<CompetitionCategory> CompetitionCategories { get; } = new List<CompetitionCategory>();
    }

    public class Discipline
    {
        public Discipline(int year, CompetitionType discipline, BowType bow, Category category, Sexe sexe, int idCateg)
        {
            throw new NotImplementedException();
        }
    }

    internal class BowTypeCategory
    {
        public BowType Bow { get; set; }

        public Category Category { get; set; }

        public BowTypeCategory(BowType bow, Category category)
        {
            this.Bow = bow;
            this.Category = category;
        }
    }
}