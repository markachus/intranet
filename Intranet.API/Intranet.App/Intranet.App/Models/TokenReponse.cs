using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.App.Models
{
    public class TokenResponse
    {

        public TokenResponse()
        {
            _tokenCreation = DateTime.Now;
        }

        private DateTime _tokenCreation;

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        public bool IsExpired { 
            get {

                var span = DateTime.Now - _tokenCreation;
                return (span.TotalSeconds >= ExpiresIn);
            } 
        }

    }
}
