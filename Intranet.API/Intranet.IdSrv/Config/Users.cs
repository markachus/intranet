using IdentityServer3.Core.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using IdentityServer3.Core;
using static IdentityServer3.Core.Constants;

namespace Intranet.IdSrv.Config
{
    public class Users
    {
        public static List<InMemoryUser> Get(){
            return new List<InMemoryUser>()
            {
                new InMemoryUser { 
                    Username="mef",
                    Password="secret",
                    Subject = "1",
                    Claims = new[]{ 
                            new Claim(IdentityServer3.Core.Constants.ClaimTypes.GivenName, "Marc"),
                            new Claim(IdentityServer3.Core.Constants.ClaimTypes.FamilyName, "Esteve")
                    }
                },
                new InMemoryUser {
                    Username="dga",
                    Password="secret",
                    Subject = "2",
                    Claims = new[]{
                            new Claim(IdentityServer3.Core.Constants.ClaimTypes.GivenName, "Daniel"),
                            new Claim(IdentityServer3.Core.Constants.ClaimTypes.FamilyName, "García")
                    }
                },

                new InMemoryUser {
                    Username="jsl",
                    Password="secret",
                    Subject = "3",
                    Claims = new[]{
                            new Claim(IdentityServer3.Core.Constants.ClaimTypes.GivenName, "Judà"),
                            new Claim(IdentityServer3.Core.Constants.ClaimTypes.FamilyName, "Sanz")
                    }
                }

            };
        }
    }
}