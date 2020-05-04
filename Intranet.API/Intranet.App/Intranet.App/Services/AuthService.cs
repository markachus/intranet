using Intranet.App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Intranet.App.Services
{
    public class AuthService : IAuthService
    {
        public AuthService()
        {
        }

        public TokenResponse TokenReponse { get; private set; }

        public async Task<TokenResponse> GetTokenAsync(string username, string password)
        {
            if (TokenReponse == null)
            {

                using (var client = new HttpClient())
                {

                    var basicAuth = Convert.ToBase64String(
                        Encoding.UTF8.GetBytes("intranet_resourceowner:secret"));

                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basicAuth);

                    var requestToken = await client.PostAsync("https://markachusidsrv.azurewebsites.net/identity/connect/token",
                                        new FormUrlEncodedContent(
                                            new[] {
                                                new KeyValuePair<string, string>(
                                                    "grant_type",
                                                    "password"),
                                                new KeyValuePair<string, string>(
                                                    "scope",
                                                    "openid profile offline_access intranetapi"),
                                                new KeyValuePair<string, string>(
                                                    "username",
                                                    username),
                                                new KeyValuePair<string, string>(
                                                    "password",
                                                    password),
                                            }
                                        ));
                    var jsonData = await requestToken.Content.ReadAsStringAsync();
                    TokenReponse =  JsonConvert.DeserializeObject<TokenResponse>(jsonData);
                }

            }

            return TokenReponse;
        }

        public async Task<TokenResponse> GetTokenRefreshTokenAsync(string refreshToken) {

            using (var client = new HttpClient())
            {

                var basicAuth = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes("intranet_resourceowner:secret"));

                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basicAuth);

                var requestToken = await client.PostAsync("https://markachusidsrv.azurewebsites.net/identity/connect/token",
                                    new FormUrlEncodedContent(
                                        new[] {
                                                new KeyValuePair<string, string>(
                                                    "grant_type",
                                                    "refresh_token"),
                                                new KeyValuePair<string, string>(
                                                    "scope",
                                                    "openid profile offline_access intranetapi"),
                                                new KeyValuePair<string, string>(
                                                    "refresh_token",
                                                    refreshToken),
                                        }
                                    ));
                var jsonData = await requestToken.Content.ReadAsStringAsync();
                TokenReponse = JsonConvert.DeserializeObject<TokenResponse>(jsonData);
            }

            return this.TokenReponse;
        }

    }
}
