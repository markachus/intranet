using IntranetCore.Data.ResourceParameters;
using IntranetCore.WebClient.Models;
using Marvin.StreamExtensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace IntranetCore.WebClient.Clients
{
    public class TagsClient
    {
        private readonly HttpClient client;


        public TagsClient(HttpClient _client)
        {
            this.client = _client;
            this.client.BaseAddress = new Uri("http://localhost:51044");
            this.client.Timeout = new TimeSpan(0, 0, 30);
            this.client.DefaultRequestHeaders.Clear();
        }


        public async Task<IEnumerable<EtiquetaModel>> GetTags(EtiquetasResourceParameters etiquetaParameters, CancellationToken cancellationToken)
        {

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, new Uri("api/v1/tags");
            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            var response = await client.SendAsync(
                                request,
                                HttpCompletionOption.ResponseHeadersRead,
                                cancellationToken);

            response.EnsureSuccessStatusCode();

            var streamContent = await response.Content.ReadAsStreamAsync();
            IEnumerable<EtiquetaModel> res = streamContent.ReadAndDeserializeFromJson<IEnumerable<EtiquetaModel>>();

            return res;
        }
    }
}
