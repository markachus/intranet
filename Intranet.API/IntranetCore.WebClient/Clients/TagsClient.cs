using IntranetCore.Data.ResourceParameters;
using IntranetCore.WebClient.Models;
using Marvin.StreamExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace IntranetCore.WebClient.Clients
{
    public class TagsClient
    {
        private readonly HttpClient client;
        public string Message { get; set; }

        public TagsClient(HttpClient _client)
        {
            this.client = _client;
            this.client.BaseAddress = new Uri("http://localhost:51044");
            this.client.Timeout = new TimeSpan(0, 0, 30);
            this.client.DefaultRequestHeaders.Clear();
        }


        public async Task<EtiquetaModel> GetTag(string tagName, CancellationToken cancellationToken)
        {

            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get, $"api/v1/tags/{tagName}");

            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.intranet.etiqueta.v1+json"));


            var response = await client.SendAsync(
                                request,
                                HttpCompletionOption.ResponseHeadersRead,
                                cancellationToken);

            switch (response.StatusCode) {
                case System.Net.HttpStatusCode.NotFound:
                    return await Task.FromResult<EtiquetaModel>(null);
            }

            var streamContent = await response.Content.ReadAsStreamAsync();
            EtiquetaModel res = streamContent.ReadAndDeserializeFromJson<EtiquetaModel>();

            return res;
        }

        public async Task<IEnumerable<EtiquetaModel>> GetTags(EtiquetasResourceParameters etiquetaParameters, CancellationToken cancellationToken)
        {

            string queryString = "";
            if (etiquetaParameters != null)
            {
                queryString = etiquetaParameters.GetQueryString();
            }

            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get, "api/v1/tags");

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


        public async Task CreateTag(EtiquetaForCreationModel tagForCreation, CancellationToken cancellationToken)
        {

            HttpRequestMessage request = new HttpRequestMessage(
              HttpMethod.Post, "api/v1/tags");

            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.intranet.etiqueta.v1+json"));


            //request.Headers.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(
                JsonConvert.SerializeObject(tagForCreation));

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.intranet.etiquetaforcreation.v1+json");

            //request.Content = new StringContent(
            //    JsonConvert.SerializeObject(tagForCreation),
            //    System.Text.Encoding.UTF8, "application/json");

            var response = await client.SendAsync(
                                request,
                                HttpCompletionOption.ResponseHeadersRead,
                                cancellationToken);

            if (!response.IsSuccessStatusCode) { 
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Message = await response.Content.ReadAsStringAsync();
                }
            }

        }

        public async Task PartialUpdate(EtiquetaModel tagForUpdate, CancellationToken cancellationToken) {

            HttpRequestMessage request = new HttpRequestMessage(
              HttpMethod.Patch, $"api/v1/tags/{tagForUpdate.Nombre}");

            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            JsonPatchDocument<EtiquetaModel> jsonPatchDoc = new JsonPatchDocument<EtiquetaModel>();
            jsonPatchDoc.Replace(e => e.HexColor, tagForUpdate.HexColor);
            jsonPatchDoc.ApplyTo(tagForUpdate);

            request.Content = new StringContent(
                JsonConvert.SerializeObject(jsonPatchDoc),
                System.Text.Encoding.UTF8, "application/json-patch+json");


            var response = await client.SendAsync(
                                request,
                                HttpCompletionOption.ResponseHeadersRead,
                                cancellationToken);

            response.EnsureSuccessStatusCode();

        }


        public async Task DeleteTag(string tagName, CancellationToken cancellationToken)
        {

            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Delete, $"api/v1/tags/{tagName}");


            var response = await client.SendAsync(
                                request,
                                HttpCompletionOption.ResponseHeadersRead,
                                cancellationToken);


            if (!response.IsSuccessStatusCode) {

                if (response.StatusCode == System.Net.HttpStatusCode.Conflict) {
                    throw new ApplicationException("Tag is being used");
                }
            }
        }

    }
}
