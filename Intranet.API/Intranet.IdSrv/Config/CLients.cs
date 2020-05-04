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
                    ClientSecrets = new List<Secret>(){ 
                        new Secret("secret".Sha256())  
                    },
                    Flow = Flows.ResourceOwner,
                    RequireConsent = true,
                    AllowedScopes = new List<string>{
                        IdentityServer3.Core.Constants.StandardScopes.OpenId,
                        "intranetapi"
                    },
                    RedirectUris = new List<string>
                    {
                        Constants.IdSrvUri
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        Constants.IdSrvUri
                    },

                },
                new Client {
                    Enabled = true,
                    ClientId = "native_implicit",
                    ClientName = "Native Mobile",
                    RequireConsent = true,
                    ClientSecrets = new List<Secret>(){
                        new Secret("secret".Sha256())
                    },
                    Flow = Flows.Implicit,
                    AllowedScopes = new List<string>{
                        IdentityServer3.Core.Constants.StandardScopes.OpenId,
                        IdentityServer3.Core.Constants.StandardScopes.Profile,
                        "intranetapi",
                        "read"
                    },
                    RedirectUris = new List<string>
                    {
                        Constants.IdSrvUri,
                        Constants.IdSrvUri
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        Constants.IdSrvUri
                    } 
                },
                new Client {
                    Enabled = true,
                    ClientId = "intranet_resourceowner",
                    ClientName = "Intramet",
                    ClientSecrets = new List<Secret>(){
                        new Secret("secret".Sha256())
                    },
                    Flow = Flows.ResourceOwner,
                    AllowedScopes = new List<string>{
                        IdentityServer3.Core.Constants.StandardScopes.OpenId,
                        IdentityServer3.Core.Constants.StandardScopes.Profile,
                        IdentityServer3.Core.Constants.StandardScopes.OfflineAccess,
                        "intranetapi"
                    }
                }
            };
        }
    }
}