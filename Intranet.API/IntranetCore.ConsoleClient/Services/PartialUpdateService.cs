using IntranetCore.ConsoleClient.Models;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntranetCore.ConsoleClient.Services
{
    public class PartialUpdateService : IIntegrationService
    {

        private static HttpClient _client = new HttpClient();

        public PartialUpdateService()
        {
            _client.BaseAddress = new Uri("http://localhost:51044/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Clear();
        }



        async Task IIntegrationService.Run()
        {
            PartialUpdateTag();
        }


        public async void PartialUpdateTag() {

            var jsonPathDoc = new JsonPatchDocument<EtiquetaForUpdateModel>();
            jsonPathDoc.Replace(p => p.HexColor, "#FF0033");

            var serializedChangedSet = new StringContent(JsonConvert.SerializeObject(jsonPathDoc));
            var requestMessage = new HttpRequestMessage(
                HttpMethod.Patch, 
                "api/v1/tags/Prestige");

            requestMessage.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = serializedChangedSet;
            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json-patch+json");
            var responseMessage = await _client.SendAsync(requestMessage);

            var content = await responseMessage.Content.ReadAsStringAsync();

        }
    }
}
