namespace FftaExtract.Web.Controllers
{
    using System;
    using System.Collections;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web.Http;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using HtmlAgilityPack;

    public class ResultatController : ApiController
    {
        private readonly CompetitionCategorieRepository competitionCategorieRepository;

        private readonly Job job;

        public ResultatController(Job job, CompetitionCategorieRepository competitionCategorieRepository)
        {
            this.job = job;
            this.competitionCategorieRepository = competitionCategorieRepository;
        }

        public void Get()
        {
            var beginDate = DateTime.Now.AddDays(-30);
            var endDate = DateTime.Now;

            foreach (var type in this.competitionCategorieRepository.GetCompetitionTypes())
            {
                this.job.Push("/api/resultat/{0}/{1:yyyy-MM-dd}/{2:yyyy-MM-dd}", type.Code, beginDate, endDate);
            }
        }

        public async Task Get(string code, DateTime beginDate, DateTime endDate)
        {
            var urlFormat =
                "http://ffta-public.cvf.fr/servlet/ResAffichListeEpreuves?TYPEAFFICHAGE=RESULTATS&DISCIP={0}&CHP=&DATE_DEB={1:dd}%2F{1:MM}%2F{1:yyyy}&DATE_FIN={2:dd}%2F{2:MM}%2F{2:yyyy}";
            var url = string.Format(urlFormat, code, beginDate, endDate);

            var client = new HttpClient();

            var document = new HtmlDocument();

            document.LoadHtml(await client.GetStringAsync(url));

            var nodes = document.DocumentNode.SelectNodes("//td[@align='center']/a");

            foreach (var node in nodes)
            {
                var hrefValue = node.GetAttributeValue("href", string.Empty);

                var r = new Regex(@"javascript:view\('([0-9]+)'\);");

                var match = r.Match(hrefValue);
                if (match.Success)
                {
                    this.job.Push("/api/resultat/compete/{1}", code, match.Groups[1].Value);
                }
            }
        }
    }
}
