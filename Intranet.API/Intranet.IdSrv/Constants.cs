using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.IdSrv
{
    public class Constants
    {
        public const string IntranetMobileClient = "";
        public const string IsSrvIssuerUri = "https://intranetidsrv3/embedded";

        public const string IdSrvUri = "https://markachusidsrv.azurewebsites.net/identity";
        public const string IdSrvAuthorizationUri = IdSrvUri + "/connect/authorize";
        public const string IdSrvTokenUri = IdSrvUri + "/connect/token";
        public const string IdSrvUserEndpointUri = IdSrvUri + "/connect/userinfo";

    }
}