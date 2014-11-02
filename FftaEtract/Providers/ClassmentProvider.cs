namespace FftaExtract.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;

    using FftaExtract.DatabaseModel;

    using HtmlAgilityPack;

    using Ninject.Extensions.Logging;

    public class ClassmentProvider
    {
        private readonly CompetionCategorieRepository competionCategorieRepository;

        private readonly ILogger logger;

        private Job job;

        public ClassmentProvider(CompetionCategorieRepository competionCategorieRepository, ILogger logger, Job job)
        {
            this.competionCategorieRepository = competionCategorieRepository;
            this.logger = logger;
            this.job = job;
        }

        public async Task<IList<ArcherDataProvider>> GetArchers(Category cat, CompetitionType competitionType, int page)
        {
            var archers = new List<ArcherDataProvider>();
            var category = this.competionCategorieRepository.GetCategory(cat, competitionType);

            if (category == null)
            {
                return archers;
            }

            const string UrlFormat = "http://ffta-public.cvf.fr/servlet/ResAffichClassement?ANNEE={0}&DISCIP=S&TYPE=I&SELECTIF=0&NIVEAU=L&DEBUT={1}&NUMCLASS={2}";
            this.logger.Info(
                "Scrap {0} {1} from {2} to {3} ",
                category.Year,
                category.CompetitionType,
                page,
                page + 50);
            var hasArcher = false;
            var url = string.Format(UrlFormat, category.Year, page, category.IdFfta);

            var scrapUrl = await this.ScrapUrl(url, category);

            //Task.WaitAll(scrapUrl);

            foreach (var archerDataProvider in scrapUrl)
            {
                hasArcher = true;

                this.job.Push("api/Palmares/{0}/{1}/{2}", cat, competitionType, archerDataProvider.Code);
                archers.Add(archerDataProvider);
            }

            if (hasArcher)
            {
                this.job.Push("api/Competion/{0}/{1}/{2}", cat, competitionType, page + 50);
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

                    var firstName = this.GetFirstName(name);
                    var lastName = this.GetLastName(name);

                    var club = columns[4].InnerText.Trim();

                    var clubYear = new ClubDataProvider { Club = club, Year = category.Year, };

                    archers.Add(new ArcherDataProvider()
                    {
                        LastName = lastName,
                        FirstName = firstName,
                        Club =
                            new List<ClubDataProvider>()
                                {
                                    new ClubDataProvider() { Club = club, Year = category.Year }
                                },
                        Code = code,
                        Sexe = category.Sexe,
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
