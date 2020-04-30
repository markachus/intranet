using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.IdSrv.Config
{
    public class Scopes
    {
        public static IEnumerable<Scope> Get() {

            return new List<Scope>()
            {
                StandardScopes.OpenId,
                StandardScopes.Profile
            };
        }
    }
}