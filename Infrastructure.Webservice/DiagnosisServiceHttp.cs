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
    public class DiagnosisServiceHttp : IDiagnosisServiceHttp
    {
        private string JWT_Token = Environment.GetEnvironmentVariable("JWT_Token");
        private const string BASE_URL = "https://avansfysiotherapyapi.azurewebsites.net/api";
        private readonly HttpClient _client = new();

        public DiagnosisServiceHttp()
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", JWT_Token);
        }

        public List<string> GetBodyLocalizations()
        {
            var response = _client.GetAsync($"{BASE_URL}/BodyLocalizations").Result;
            var bodyLocalizations =
                JsonConvert.DeserializeObject<List<string>>(response.Content.ReadAsStringAsync().Result);
            return bodyLocalizations;
        }

        public Diagnosis GetDiagnosis(int id)
        {
            var response = _client.GetAsync($"{BASE_URL}/Diagnoses/{id}").Result;
            var diagnose = JsonConvert.DeserializeObject<Diagnosis>(response.Content.ReadAsStringAsync().Result);
            return diagnose;
        }

        public List<Diagnosis> GetDiagnosesByBodyLocalization(string bodyLocalization)
        {
            var response = _client.GetAsync($"{BASE_URL}/Diagnoses/Category/{bodyLocalization}").Result;
            var diagnoses = JsonConvert.DeserializeObject<List<Diagnosis>>(response.Content.ReadAsStringAsync().Result);
            return diagnoses;
        }
    }
}
