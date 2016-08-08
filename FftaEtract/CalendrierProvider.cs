namespace FftaExtract
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using FftaExtract.DatabaseModel;
    using FftaExtract.Providers;

    using HtmlAgilityPack;

    using Ninject.Extensions.Logging;

    public class CalendrierProvider
    {
        public CalendrierProvider(object o, ILogger @for, object o1)
        {
        }

        public async Task<IList<CompetitionDataProviderBase>> ScrapUrl(CompetitionType competitionType)
        {
            var client = new HttpClient();
            var content =
                new StringContent(
                    $"ChxDiscipline={competitionType.GetCode()}&ChxDateDebut=08%2F08%2F2015&ChxDateFin=08%2F08%2F2015", Encoding.UTF8, "application/x-www-form-urlencoded");

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

                var dateBegin = DateTime.ParseExact(
                    matchDate.Groups[1].Value,
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture);
                var dateEnd = DateTime.ParseExact(
                    matchDate.Groups[2].Value,
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture);

                var nameB = competitionDiv.SelectSingleNode("div[@class='info']/b");
                var yearI = competitionDiv.SelectSingleNode("div[@class='info']/i");
                var year = Regex.Match(yearI.InnerText, "[0-9]+").Groups[0].Value;
                competitions.Add(new CompetitionDataProviderBase(Convert.ToInt32(year), dateBegin, dateEnd, competitionType, nameB.InnerText));
            }

            return competitions;
        }
    }
}