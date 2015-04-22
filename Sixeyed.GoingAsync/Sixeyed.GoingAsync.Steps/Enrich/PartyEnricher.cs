using Newtonsoft.Json.Linq;
using Sixeyed.GoingAsync.Core;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Sixeyed.GoingAsync.Steps.Enrich
{
    public class PartyEnricher
    {
        private static string _BaseUrl;

        static PartyEnricher()
        {
            _BaseUrl = Config.Get("PartyEnricher.ApiBaseUrl");
        }

        public string GetInternalId(string legalEntityIdentifier)
        {
            var requestUrl = string.Format("{0}/party?id={1}&scheme=lei", _BaseUrl, legalEntityIdentifier);
            using (var apiClient = new HttpClient())
            {
                apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = apiClient.GetStringAsync(requestUrl).Result;
                dynamic party = JObject.Parse(json);
                return (string)party.internalId;
            }
        }
    }
}
