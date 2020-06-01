using IntranetCore.ConsoleClient.Models;
using Marvin.StreamExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IntranetCore.ConsoleClient.Services
{
    class DealingWithErrorsAndFaultService : IIntegrationService
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly IHttpClientFactory _httpClientFactory;

        async Task IIntegrationService.Run()
        {
            //await GetRequestWithWrongUri(_cancellationTokenSource.Token);
            await PosstTagWithInvalidData(_cancellationTokenSource.Token);
        }

        public DealingWithErrorsAndFaultService(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        private async Task GetRequestWithWrongUri(CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient("TagsClient");

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "/api/v1/tagss");

            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (var response = await httpClient.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken))
            {

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        Console.WriteLine("Uri not found");
                        return;

                    }
                    else { 
                        response.EnsureSuccessStatusCode();
                    }
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var tags = stream.ReadAndDeserializeFromJson<IEnumerable<EtiquetaModel>>();
            }
        }

        private async Task PosstTagWithInvalidData(CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient("TagsClient");

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "api/v1/tags");


            var newTag = new EtiquetaForCreationModel()
            {
                Nombre = "test",
                HexColor = "invaluid hex color"
            };



            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.intranet.etiqueta.v1+json"));
            //request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            request.Content = new StringContent(JsonConvert.SerializeObject(newTag));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.intranet.etiquetaforcreation.v1+json");


            using (var response = await httpClient.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken))
            {

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                    {

                        var streamErrors = await response.Content.ReadAsStreamAsync();
                        var s = streamErrors.ReadAndDeserializeFromJson();
                        Console.WriteLine(s);
                        return;

                    }
                    else
                    {
                        response.EnsureSuccessStatusCode();
                    }
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var tags = stream.ReadAndDeserializeFromJson<IEnumerable<EtiquetaModel>>();
            }
        }



    }
}
