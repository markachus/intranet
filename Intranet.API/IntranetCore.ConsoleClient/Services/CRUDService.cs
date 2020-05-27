using IntranetCore.ConsoleClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IntranetCore.ConsoleClient.Services
{
    public class CRUDService : IIntegrationService
    {

        private HttpClient _client = new HttpClient();

        public CRUDService()
        {
            _client.BaseAddress = new Uri("http://localhost:51044/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task Run()
        {
            await GetTags();
        }

        public async Task<IEnumerable<EtiquetaModel>> GetTags() {

            var response = await _client.GetAsync("api/v1/tags");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var tags = JsonConvert.DeserializeObject<IEnumerable<EtiquetaModel>>(content);

            return tags;
        }
    }
}
