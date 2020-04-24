using Intranet.App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Intranet.App.Services
{
    public class EtiquetasServices
    {

        private string _baseAddress = "https://intranetapi20200423194610.azurewebsites.net/"; //= Device.RuntimePlatform == Device.Android ? "http://10.0.2.2:8443" : "http://127.0.0.1:8443";
        private const string _url = "api/tags";

        private string _message;
        public string LastMessage { get { return _message; } }

        private HttpStatusCode _statusCode;

        public HttpStatusCode LastStatusCode
        {
            get
            {
                return _statusCode;
            }
            private set
            {
                _statusCode = value;
            }
        }


        private bool _lastRequestOk;
        public bool LastRequestOk { get => _lastRequestOk;  }

        private HttpClient GetClient()
        {

            HttpClient client = new HttpClient(GetInsecureHandler());
            return client;
        }

        public HttpClientHandler GetInsecureHandler()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }

        public async void AddEtiqueta(EtiquetaModel etiquetaModel)
        {
            HttpClient client = GetClient();
            string sTag = JsonConvert.SerializeObject(etiquetaModel);
            var resp = await client.PostAsync(
                $"{_baseAddress}/{_url}",
                new StringContent(sTag, Encoding.UTF8, "application/json"));

            this.LastStatusCode = resp.StatusCode;
            if (_lastRequestOk = resp.IsSuccessStatusCode)
            {

                this._message = string.Empty;
            }
            else
            {
                ApiUnsuccessResponse ar = JsonConvert.DeserializeObject<ApiUnsuccessResponse>(await resp.Content.ReadAsStringAsync());
                _message = ar.Message;
            }
        }

        public async Task<IEnumerable<EtiquetaModel>> GetAll()
        {

            HttpClient client = GetClient();
            var response = await client.GetAsync($"{_baseAddress}/{_url}");

            this.LastStatusCode = response.StatusCode;
            var sTags = await response.Content.ReadAsStringAsync();

            if (_lastRequestOk = response.IsSuccessStatusCode)
            {
                this._message = string.Empty;
                IEnumerable<EtiquetaModel> tags = JsonConvert.DeserializeObject<IEnumerable<EtiquetaModel>>(sTags);
                return tags;
            }
            else
            {
                ApiUnsuccessResponse ar = JsonConvert.DeserializeObject<ApiUnsuccessResponse>(sTags);
                _message = ar.Message;
                return new List<EtiquetaModel>();
            }


        }

    }
}
