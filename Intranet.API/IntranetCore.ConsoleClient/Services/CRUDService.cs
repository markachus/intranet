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

        private static HttpClient _client = new HttpClient();

        public CRUDService()
        {
            _client.BaseAddress = new Uri("http://localhost:51044/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


            _client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml", 0.9));
        }
        public async Task Run()
        {
            //await GetTags();
            await GetTagsThroughHttpMessageRequest();
        }

        public async Task<IEnumerable<EtiquetaModel>> GetTags()
        {

            var response = await _client.GetAsync("api/v1/tags");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var tags = JsonConvert.DeserializeObject<IEnumerable<EtiquetaModel>>(content);

            return tags;
        }


        public async Task<IEnumerable<EtiquetaModel>> GetTagsThroughHttpMessageRequest()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/tags");
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var sContent = await response.Content.ReadAsStringAsync();
            var tags = JsonConvert.DeserializeObject<IEnumerable<EtiquetaModel>>(sContent);

            return tags;
        }
    }
}
