using Intranet.App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<EtiquetaModel>> GetAll() {

            HttpClient client = GetClient();
            var response = await client.GetStringAsync($"{_baseAddress}/{_url}");
            IEnumerable<EtiquetaModel> tags = JsonConvert.DeserializeObject<IEnumerable<EtiquetaModel>>(response);

            return tags;
        }

    }
}
