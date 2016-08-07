namespace FftaExtract.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Policy;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;

    using FftaExtract.DatabaseModel;

    using HtmlAgilityPack;

    using Ninject.Extensions.Logging;

    public class PalmaresProvider
    {
        private readonly CompetitionCategorieRepository competitionCategorieRepository;

        private ILogger logger;

        public PalmaresProvider(CompetitionCategorieRepository competitionCategorieRepository, ILogger logger)
        {
            this.competitionCategorieRepository = competitionCategorieRepository;
            this.logger = logger;
        }

        private Random rand = new Random();


        public async Task UpdateArcher(ArcherDataProvider archer, int? year, Category? category1, CompetitionType? competitionType, BowType? bowType)
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

            bool hasError = false;

            foreach (var sex in sexes)
            {
                foreach (var category in this.competitionCategorieRepository.GetCategories(sex, year))
                {
                    if (category1.HasValue && category1.Value != category.Category)
                    {
                        continue;
                    }

                    if (competitionType.HasValue && competitionType.Value != category.CompetitionType)
                    {
                        continue;
                    }

                    if (bowType.HasValue && bowType.Value != category.BowType)
                    {
                        continue;
                    }

                    try
                    {
                        await this.ScrapUrl(category, archer);

                        Thread.Sleep(TimeSpan.FromMilliseconds(this.rand.Next(1, 500)));
                    }
                    catch (Exception ex)
                    {
                        this.logger.Error(ex, $"Error in scrap archer {archer.Code} with category {category.IdFfta}");
                        hasError = true;
                    }
                }
            }

            if (hasError)
            {
                throw new Exception("Error to scrap a competition");
            }
        }

        public async Task ScrapUrl(CompetitionCategory category, ArcherDataProvider archerDataProvider)
        {
            var client = new HttpClient();
            var content =
                new StringContent(
                    $"operation=clsPers&ClassementId={category.IdFfta}&PersonneId={archerDataProvider.Code}", Encoding.UTF8, "application/x-www-form-urlencoded");
            var result = await client.PostAsync("http://classements.ffta.fr/actions/outils/AjaxPalmares.php", content);

            HtmlDocument doc = new HtmlDocument();
            doc.Load(await result.Content.ReadAsStreamAsync(), Encoding.UTF8);

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
            var grostitre = doc.DocumentNode.SelectNodes("//tr[@class='clmt']/td[@colspan='2']/b");
            int year = 0;

            if (grostitre != null)
            {
                foreach (var titre in grostitre)
                {
                    year = Convert.ToInt32(Regex.Match(titre.InnerText.Trim(), "[0-9]+$").Groups[0].Value);
                }
            }
            else
            {
                grostitre = doc.DocumentNode.SelectNodes("//tr[@class='clmt']/td/strong");
                if (grostitre != null)
                {
                    foreach (var titre in grostitre)
                    {
                        year = Convert.ToInt32(Regex.Match(titre.InnerText.Trim(), "[0-9]+$").Groups[0].Value);
                    }
                }
                else
                {
                    var h3 = doc.DocumentNode.SelectNodes("//h3");

                    if (h3 != null)
                    {
                        foreach (var titre in h3)
                        {
                            year = Convert.ToInt32(Regex.Match(titre.InnerText.Trim(), "[0-9]+$").Groups[0].Value);
                        }
                    }
                }
            }


            var tables = doc.DocumentNode.SelectNodes("//table");

            if (tables != null)
            {
                foreach (HtmlNode table in tables)
                {
                    var trs = table.SelectNodes("//tr");

                    if (trs != null)
                    {
                        foreach (var tr in trs.Skip(2))
                        {
                            var td = tr.SelectNodes("td");

                            if (td.Count == 3)
                            {
                                //continue;
                            }

                            var dateText = WebUtility.HtmlDecode(td[0].InnerText).Trim();

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
                            else
                            {
                                matchText = Regex.Match(
                                        dateText,
                                        "^([0-9]{1,2}/[0-9]{1,2}/[0-9]{4})$");

                                if (matchText.Success)
                                {
                                    begin = DateTime.ParseExact(
                                        matchText.Groups[1].Value,
                                        new string[] { "dd/MM/yyyy", "d/MM/yyyy" },
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None);
                                    end = begin;
                                }
                            }

                            var name = td[1].InnerText;
                            var score = td.Count == 4 ? td[3].InnerText : td[2].InnerText;
                            var rankString = td.Count == 4 ? td[2].InnerText : "0";
                            int rank;
                            if (!int.TryParse(rankString, out rank))
                            {
                                rank = 0;
                            }

                            var scores = ExtractScore(score);
                            foreach (var score1 in scores)
                            {
                                competitions.Add(
                                    new CompetitionDataProvider(year, begin.Date, end.Date,
                                        td[1].InnerText.Trim(),
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

            match = Regex.Match(score, @"^([0-9]+)\s*pts\s*$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                yield return Convert.ToInt32(match.Groups[1].Value);
                yield break;
            }

            match = Regex.Match(score, @"^([0-9]+)\s*\(Pas pris en compte\)\s*$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                yield return Convert.ToInt32(match.Groups[1].Value);
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