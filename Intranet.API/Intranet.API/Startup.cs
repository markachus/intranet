using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(Intranet.API.Startup))]
namespace Intranet.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            UnityWebApiActivator.Start();
            GlobalConfiguration.Configure(WebApiConfig.Register);

        }

    }
}