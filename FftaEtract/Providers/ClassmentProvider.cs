using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FftaExtract.Providers
{
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Web;

    using HtmlAgilityPack;

    public class ClassmentProvider : IStatsProvider
    {
        private CompationCategorieRepository compationCategorieRepository;

        public ClassmentProvider(CompationCategorieRepository compationCategorieRepository)
        {
            this.compationCategorieRepository = compationCategorieRepository;
        }

        public async Task<IList<ArcherDataProvider>> GetArchers()
        {
            var archers = new List<ArcherDataProvider>();

            var urlFormat = "http://ffta-public.cvf.fr/servlet/ResAffichClassement?ANNEE={0}&DISCIP=S&TYPE=I&SELECTIF=0&NIVEAU=L&DEBUT=0&NUMCLASS={1}";

            foreach (var category in this.compationCategorieRepository.GetCategories())
            {
                var url = string.Format(urlFormat, category.Year, category.IdFfta);

                var scrapeArchers = await this.ScrapUrl(url, category);

                archers.AddRange(scrapeArchers);
            }

            return archers;
        }


        private async Task<IList<ArcherDataProvider>> ScrapUrl(string url, CompetitionCategory category)
        {
            var archers = new List<ArcherDataProvider>();

            var client = new HttpClient();
            var result = await client.GetAsync(url);

            HtmlDocument doc = new HtmlDocument();
            doc.Load(await result.Content.ReadAsStreamAsync());

            var archersLines = doc.DocumentNode.SelectNodes("//table[@class='texteMoteur']//tr");

            if (archersLines != null)
            {
                foreach (var archerLine in archersLines.Skip(3))
                {
                    var columns = archerLine.SelectNodes("td");

                    var htmlNode = archerLine.Descendants("a").First();
                    var href = htmlNode.Attributes["href"].Value;
                    var code = Regex.Match(href, @"^JavaScript:view_fiche\('([0-9]+)'\);$").Groups[1].Value;

                    var name = HttpUtility.HtmlDecode(columns[3].InnerText).Trim();

                    var firstName = GetFirstName(name);
                    var lastName = GetLastName(name);

                    var club = columns[4].InnerText.Trim();

                    archers.Add(new ArcherDataProvider()
                    {
                        LastName = lastName,
                        FirstName = firstName,
                        Club = club,
                        Code = code,
                    });
                }
            }

            return archers;
        }

        private string GetLastName(string name)
        {
            var words = from w in name.Split(' ') where w.ToUpperInvariant() == w select w;

            return string.Join(" ", words.ToArray());
        }

        private string GetFirstName(string name)
        {
            var words = from w in name.Split(' ') where w.ToUpperInvariant() != w select w;

            return string.Join(" ", words.ToArray());
        }
    }
}
