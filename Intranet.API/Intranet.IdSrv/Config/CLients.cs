using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.IdSrv.Config
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new[] {
                new Client { 
                    Enabled = true,
                    ClientId = "Native",
                    ClientName = "Native Mobile",
                    ClientSecrets = new List<Secret>(){ 
                        new Secret("secret".Sha256())  
                    },
                    Flow = Flows.ResourceOwner,
                    RequireConsent = true,
                    RedirectUris = new List<string>{ Constants.IntranetMobileClient},
                    AllowedScopes = new List<string>{
                        IdentityServer3.Core.Constants.StandardScopes.OpenId
                    }
                }
            };
        }
    }
}