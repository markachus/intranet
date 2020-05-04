using Akavache;
using CacheCow.Client;
using Intranet.App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Reactive.Linq;

namespace Intranet.App.Services
{
    public class EtiquetasServices: IEtiquetaService
    {
        public EtiquetasServices()
        {

        }

        private string _baseAddress = "https://intranetapi20200424201859.azurewebsites.net"; //= Device.RuntimePlatform == Device.Android ? "http://10.0.2.2:8443" : "http://127.0.0.1:8443";
        private const string _url = "api/tags";

        private string _message;
        string IEtiquetaService.LastMessage { get { return _message; } }

        private HttpStatusCode _statusCode;

        HttpStatusCode IEtiquetaService.LastStatusCode
        {
            get
            {
                return _statusCode;
            }
        }


        private bool _lastRequestOk;
        bool IEtiquetaService.LastRequestOk { get => _lastRequestOk;  }

        private HttpClient GetClient()
        {

            HttpClient client = new HttpClient(new CachingHandler { 
                InnerHandler = new HttpClientHandler()
            });

            var authService = DependencyService.Get<AuthService>();
            var token = authService.TokenReponse;

            if (string.IsNullOrEmpty(token?.AccessToken)) throw new UnauthorizedAccessException("No se encontró token access");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.AccessToken);
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        }


        private async Task<HttpClient> GetClientAsync()
        {

            HttpClient client = new HttpClient(new CachingHandler
            {
                InnerHandler = new HttpClientHandler()
            });

            var authService = DependencyService.Get<AuthService>();
            var token = authService.TokenReponse;

            if (string.IsNullOrEmpty(token?.AccessToken)) throw new UnauthorizedAccessException("No se encontró token access");

            if (token.IsExpired)
            {
                token = await authService.GetTokenRefreshTokenAsync(token.RefreshToken);
            }

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.AccessToken);
            client.DefaultRequestHeaders.Add("Accept", "application/json");

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

        async void IEtiquetaService.Update(EtiquetaModel model) {
            try
            {
                HttpClient client = GetClient();
                var json = JsonConvert.SerializeObject(model);
                var resp = await client.PutAsync($"{_baseAddress}/{_url}/{model.Nombre}", 
                                    new StringContent(json, Encoding.UTF8, "application/json"));
                
                this._statusCode = resp.StatusCode;
                if (resp.IsSuccessStatusCode)
                {
                    this._message = string.Empty;
                }
                else
                {
                    ApiUnsuccessResponse ar = JsonConvert.DeserializeObject<ApiUnsuccessResponse>(await resp.Content.ReadAsStringAsync());
                    _message = ar.Message;
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error desconocido",
                    ex.Message,
                    "OK");
            }
        }

        async void IEtiquetaService.Delete(string nombre) {
            try
            {
                HttpClient client = GetClient();
                var resp = await client.DeleteAsync($"{_baseAddress}/{_url}/{nombre}");
                this._statusCode = resp.StatusCode;
                if (resp.IsSuccessStatusCode)
                {
                    this._message = string.Empty;
                }
                else {
                    ApiUnsuccessResponse ar = JsonConvert.DeserializeObject<ApiUnsuccessResponse>(await resp.Content.ReadAsStringAsync());
                    _message = ar.Message;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error desconocido", 
                    ex.Message, 
                    "OK");
            }
        }

        async void IEtiquetaService.Add(EtiquetaModel etiquetaModel)
        {
            HttpClient client = GetClient();
            string sTag = JsonConvert.SerializeObject(etiquetaModel);
            var resp = await client.PostAsync(
                $"{_baseAddress}/{_url}",
                new StringContent(sTag, Encoding.UTF8, "application/json"));

            this._statusCode = resp.StatusCode;
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

        async Task<IEnumerable<EtiquetaModel>> IEtiquetaService.GetAll()
        {

            IEnumerable<EtiquetaModel> tags = null;
            //var cache = BlobCache.InMemory;
            //var cachedTags = cache.GetAndFetchLatest("tags", () => GetAllRemote(), offset =>
            //{
            //    TimeSpan elapse = DateTimeOffset.Now - offset;
            //    return elapse > new TimeSpan(hours: 0, minutes: 1, seconds: 0);
            //});

            //cachedTags.Subscribe((updateRes) =>
            //{
            //    tags = updateRes;    
            //});

            //tags = await cachedTags.FirstOrDefaultAsync();
            tags = await GetAllRemote();
            return tags;
        }

        async Task<IEnumerable<EtiquetaModel>> GetAllRemote()
        {

            HttpClient client = await GetClientAsync();
            var response = await client.GetAsync($"{_baseAddress}/{_url}");

            this._statusCode = response.StatusCode;
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
