namespace FftaEtract
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;

    using HtmlAgilityPack;

    internal class PalmaresProvider : IStatsProvider
    {
        private IRepository repository;

        private CompetitionCategory[] categories = new[]
                                       {
                                           //3393, // SH CL 2009
                                          new CompetitionCategory(CompetitionType.Salle, BowType.Classique, 6802), // CompetitionType SH CL 2014
                                          new CompetitionCategory(CompetitionType.Fita, BowType.Classique, 7361), // FITA SH CL 2014
                                           //3394, // SF CL 2009
                                       };

        public PalmaresProvider(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IList<ArcherDataProvider>> GetArchers()
        {
            var archers = new List<ArcherDataProvider>();

            foreach (var archer in this.repository.GetAllArchers())
            {
                foreach (var category in this.categories)
                {
                    var url = string.Format("http://ffta-public.cvf.fr/servlet/ResPalmares?NUM_ADH={0}&CLASS_SELECT={1}", archer.Num, category.IdFfta);

                    await this.ScrapUrl(url, category, archer);
                }

                archers.Add(archer);
            }

            return archers;
        }

        private async Task ScrapUrl(string url, CompetitionCategory category, ArcherDataProvider archerDataProvider)
        {

            var client = new HttpClient();
            var result = await client.GetAsync(url);

            HtmlDocument doc = new HtmlDocument();
            doc.Load(await result.Content.ReadAsStreamAsync());

            ScrapArcher(archerDataProvider, doc);

            var competitions = ScrapCompetition(doc, category);

            foreach (var competition in competitions)
            {
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

                            var name = td[2].InnerText;
                            var score = td[4].InnerText;

                            competitions.Add(
                                new CompetitionDataProvider(year,
                                    td[2].InnerText.Trim(),
                                    category.CompetitionType, category.BowType, Convert.ToInt32(td[4].InnerText.Trim())));
                        }
                    }
                }
            }

            return competitions;
        }

        private static void ScrapArcher(ArcherDataProvider archerDataProvider, HtmlDocument doc)
        {
            if (string.IsNullOrEmpty(archerDataProvider.LastName) && string.IsNullOrEmpty(archerDataProvider.FirstName))
            {
                var tdName = doc.DocumentNode.SelectNodes("//td[@class='titreactu']");

                if (tdName != null)
                {
                    foreach (var td in tdName)
                    {
                        var name = HttpUtility.HtmlDecode(td.InnerText).Trim();

                        archerDataProvider.LastName = name.Split(' ')[0];
                        archerDataProvider.FirstName = name.Split(' ')[1];
                    }
                }
            }
        }
    }
}