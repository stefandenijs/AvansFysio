using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Webservice;
using Core.DomainServices;
using Newtonsoft.Json;

namespace Infrastructure.Webservice
{
    public class TreatmentInfoServiceHttp : ITreatmentInfoServiceHttp
    {
        private string JWT_Token = Environment.GetEnvironmentVariable("JWT_Token");
        private const string BASE_URL = "https://avansfysiotherapyapi.azurewebsites.net/api";
        private readonly HttpClient _client = new();

        public TreatmentInfoServiceHttp()
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", JWT_Token);
        }

        public TreatmentInfo GetTreatmentInfo(int id)
        {
            var response = _client.GetAsync($"{BASE_URL}/Treatments/{id}").Result;
            var treatmentInfo =
                JsonConvert.DeserializeObject<TreatmentInfo>(response.Content.ReadAsStringAsync().Result);
            return treatmentInfo;
        }

        public List<TreatmentInfo> GetAllTreatmentInfo()
        {
            var response = _client.GetAsync($"{BASE_URL}/Treatments").Result;
            var allTreatmentInfo =
                JsonConvert.DeserializeObject<List<TreatmentInfo>>(response.Content.ReadAsStringAsync().Result);
            return allTreatmentInfo;
        }
    }
}
