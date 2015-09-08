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
        private readonly CompetitionCategorieRepository competitionCategorieRepository;

        private readonly ILogger logger;

        private Job job;

        public ClassmentProvider(CompetitionCategorieRepository competitionCategorieRepository, ILogger logger, Job job)
        {
            this.competitionCategorieRepository = competitionCategorieRepository;
            this.logger = logger;
            this.job = job;
        }

        public async Task<IList<ArcherDataProvider>> GetArchers(int year, Category cat, CompetitionType competitionType, BowType bowType)
        {
            var archers = new List<ArcherDataProvider>();
            var category = this.competitionCategorieRepository.GetCategory(year, cat, competitionType, bowType);

            if (category == null)
            {
                return archers;
            }

            const string UrlFormat = "http://classements.ffta.fr/iframe/classements/{0}.html";
            this.logger.Info(
                "Scrap {0} {1}",
                category.Year,
                category.CompetitionType);
            var url = string.Format(UrlFormat, category.IdFfta);

            var scrapUrl = await this.ScrapUrl(url, category);

            foreach (var archerDataProvider in scrapUrl)
            {

                this.job.Push("api/Palmares/{0}/{1}/{2}/{3}/{4}", archerDataProvider.Code, category.Year, cat, competitionType, bowType);
                archers.Add(archerDataProvider);
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

            var archersLines = doc.DocumentNode.SelectNodes("//table[@class='crh clmt']//tr");

            if (archersLines != null)
            {
                foreach (var archerLine in archersLines.Skip(1))
                {
                    var columns = archerLine.SelectNodes("td");

                    var code = columns[3].InnerText;

                    var name = HttpUtility.HtmlDecode(columns[4].InnerText).Trim();

                    var firstName = this.GetFirstName(name);
                    var lastName = this.GetLastName(name);


                    var club = string.Empty;

                    int v;
                    var col6 = columns[6].InnerText.Replace("&nbsp;", string.Empty);
                    if (!int.TryParse(col6.Trim(), out v))
                    {
                        club = col6.Trim();                        
                    }
                    else
                    {
                        club = columns[5].InnerText.Trim().Replace("&nbsp;", string.Empty);
                    }

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
