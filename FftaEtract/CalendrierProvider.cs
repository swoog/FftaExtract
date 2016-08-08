namespace FftaExtract
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using HtmlAgilityPack;

    public class CalendrierProvider
    {
        private readonly IRepositoryImporter repositoryImporter;

        public CalendrierProvider(IRepositoryImporter repositoryImporter)
        {
            this.repositoryImporter = repositoryImporter;
        }

        public async Task<IList<CompetitionDataProviderBase>> ScrapUrl(CompetitionType competitionType, DateTime beginDate, DateTime endDate)
        {
            var client = new HttpClient();
            var beginDateString = HttpUtility.UrlEncode(beginDate.ToString("dd/MM/yyyy"));
            var endDateString = HttpUtility.UrlEncode(endDate.ToString("dd/MM/yyyy"));
            var content =
                new StringContent(
                    $"ChxDiscipline={competitionType.GetCode()}&ChxDateDebut={beginDateString}&ChxDateFin={endDateString}", Encoding.UTF8, "application/x-www-form-urlencoded");

            var result = await client.PostAsync("http://www.evenements-sportifs.com/ffta-fr/www/calendrier/index.htm", content);

            HtmlDocument doc = new HtmlDocument();
            doc.Load(await result.Content.ReadAsStreamAsync(), Encoding.UTF8);

            var competitionsDiv = doc.DocumentNode.SelectNodes("//div[@class='pave_eprv']");
            var competitions = new List<CompetitionDataProviderBase>();

            foreach (var competitionDiv in competitionsDiv)
            {
                var dateDiv = competitionDiv.SelectSingleNode("div[@class='date']");
                var dateString = dateDiv.InnerText.Trim();

                var matchDate = Regex.Match(dateString, "([0-9]+/[0-9]+/[0-9]+) au ([0-9]+/[0-9]+/[0-9]+)");

                DateTime dateBegin;
                DateTime dateEnd;

                if (matchDate.Success)
                {
                    dateBegin = DateTime.ParseExact(
                        matchDate.Groups[1].Value,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture);
                    dateEnd = DateTime.ParseExact(matchDate.Groups[2].Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    matchDate = Regex.Match(dateString, "^([0-9]+/[0-9]+/[0-9]+)$");

                    dateBegin = DateTime.ParseExact(
                        matchDate.Groups[1].Value,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture);
                    dateEnd = dateBegin;
                }

                var nameB = competitionDiv.SelectSingleNode("div[@class='info']/b");
                var yearI = competitionDiv.SelectSingleNode("div[@class='info']/i");
                var year = Regex.Match(yearI.InnerText, "[0-9]+").Groups[0].Value;
                competitions.Add(new CompetitionDataProviderBase(Convert.ToInt32(year), dateBegin, dateEnd, competitionType, nameB.InnerText));
            }

            return competitions;
        }

        public async Task Import(CompetitionType competitionType, DateTime beginDate, DateTime endDate)
        {
            foreach (var competition in await this.ScrapUrl(competitionType, beginDate, endDate))
            {
                this.repositoryImporter.SaveCompetitionDetails(competition);
            }
        }
    }
}