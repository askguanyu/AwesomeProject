using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Services.InMemory;

namespace AwesomeServer.Configuration
{
    internal class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "E6C344FA-2310-4A25-81E4-694B7A122682",
                    Username = "demo@demo.com",
                    Password = "!P@ssw0rd",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Name, "Demo User"),
                        new Claim(JwtClaimTypes.Email, "demo@demo.com")
                    }
                }
            };
        }
    }
}