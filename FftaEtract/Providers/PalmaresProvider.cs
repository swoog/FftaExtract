namespace FftaExtract.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Policy;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;

    using FftaExtract.DatabaseModel;

    using HtmlAgilityPack;

    using Ninject.Extensions.Logging;

    public class PalmaresProvider
    {
        private readonly CompetionCategorieRepository competionCategorieRepository;

        private ILogger logger;

        public PalmaresProvider(CompetionCategorieRepository competionCategorieRepository, ILogger logger)
        {
            this.competionCategorieRepository = competionCategorieRepository;
            this.logger = logger;
        }

        private Random rand = new Random();


        public async Task UpdateArcher(ArcherDataProvider archer)
        {
            var sexes = new List<Sexe>();

            if (archer.Sexe.HasValue)
            {
                sexes.Add(archer.Sexe.Value);
            }
            else
            {
                sexes.Add(Sexe.Homme);
                sexes.Add(Sexe.Femme);
            }

            foreach (var sex in sexes)
            {
                foreach (var category in this.competionCategorieRepository.GetCategories(sex))
                {
                    string url = string.Empty;
                    try
                    {
                        url = string.Format(
                            "http://ffta-public.cvf.fr/servlet/ResPalmares?NUM_ADH={0}&CLASS_SELECT={1}",
                            archer.Num,
                            category.IdFfta);

                        await this.ScrapUrl(url, category, archer);

                        Thread.Sleep(TimeSpan.FromMilliseconds(this.rand.Next(1, 1000)));
                    }
                    catch (Exception ex)
                    {
                        this.logger.Error(ex, "Error in scrap url : {0}", url);
                    }
                }
            }
        }

        private async Task ScrapUrl(string url, CompetitionCategory category, ArcherDataProvider archerDataProvider)
        {
            var client = new HttpClient();
            var result = await client.GetAsync(url);

            HtmlDocument doc = new HtmlDocument();
            doc.Load(await result.Content.ReadAsStreamAsync());

            //ScrapArcher(archerDataProvider, doc);

            var competitions = ScrapCompetition(doc, category);

            foreach (var competition in competitions)
            {
                archerDataProvider.Sexe = category.Sexe;
                archerDataProvider.AddCompetition(competition);
            }
        }

        private static List<CompetitionDataProvider> ScrapCompetition(HtmlDocument doc, CompetitionCategory category)
        {
            var competitions = new List<CompetitionDataProvider>();
            var grostitre = doc.DocumentNode.SelectNodes("//td[@class='grostitre']");
            int year = 0;

            if (grostitre != null)
            {
                foreach (var titre in grostitre)
                {
                    year = Convert.ToInt32(Regex.Match(titre.InnerText.Trim(), "[0-9]+$").Groups[0].Value);
                }
            }


            var tables = doc.DocumentNode.SelectNodes("//table[@class='texteMoteur']");

            if (tables != null)
            {
                foreach (HtmlNode table in tables)
                {
                    var trs = table.SelectNodes("tr");

                    if (trs != null)
                    {
                        foreach (var tr in trs.Skip(2))
                        {
                            var td = tr.SelectNodes("td");

                            var dateText = td[1].InnerText;

                            var matchText = Regex.Match(
                                dateText,
                                "^([0-9]{1,2}/[0-9]{1,2}/[0-9]{4})[^0-9]*([0-9]{1,2}/[0-9]{1,2}/[0-9]{4})$");
                            DateTime begin = DateTime.Now;
                            DateTime end = DateTime.Now;

                            if (matchText.Success)
                            {
                                begin = DateTime.ParseExact(
                                    matchText.Groups[1].Value,
                                    new string[] { "dd/MM/yyyy", "d/MM/yyyy" },
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None);
                                end = DateTime.ParseExact(
                                    matchText.Groups[2].Value,
                                    new string[] { "dd/MM/yyyy", "d/MM/yyyy" },
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None);
                            }

                            var name = td[2].InnerText;
                            var score = td[4].InnerText;
                            var rank = Convert.ToInt32(td[3].InnerText);

                            var scores = ExtractScore(score);
                            foreach (var score1 in scores)
                            {
                                competitions.Add(
                                    new CompetitionDataProvider(year, begin, end,
                                        td[2].InnerText.Trim(),
                                        category.CompetitionType, category.BowType, score1, rank));
                            }
                        }
                    }
                }
            }

            return competitions;
        }

        private static IEnumerable<int> ExtractScore(string score)
        {
            var match = Regex.Match(score, @"^([0-9]+)\s*\(([0-9]+)\s*pts\s*\+\s*([0-9]+)\s*pts\)$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                yield return Convert.ToInt32(match.Groups[2].Value);
                yield return Convert.ToInt32(match.Groups[3].Value);
                yield break;
            }

            yield return Convert.ToInt32(score.Trim());
        }

        //private static void ScrapArcher(ArcherDataProvider archerDataProvider, HtmlDocument doc)
        //{
        //    if (string.IsNullOrEmpty(archerDataProvider.LastName) && string.IsNullOrEmpty(archerDataProvider.FirstName))
        //    {
        //        var tdName = doc.DocumentNode.SelectNodes("//td[@class='titreactu']");

        //        if (tdName != null)
        //        {
        //            foreach (var td in tdName)
        //            {
        //                var name = HttpUtility.HtmlDecode(td.InnerText).Trim();

        //                archerDataProvider.LastName = name.Split(' ')[0];
        //                archerDataProvider.FirstName = name.Split(' ')[1];
        //            }
        //        }
        //    }
        //}
    }
}