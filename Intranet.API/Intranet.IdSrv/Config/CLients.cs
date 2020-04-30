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
                    ClientName = "Native",
                    Flow = Flows.Implicit,
                    RequireConsent = true,
                    RedirectUris = new List<string>{ Constants.IntranetMobileClient}
                }
            };
        }
    }
}