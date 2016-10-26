using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;

namespace AwesomeServer.Configuration
{
    public class Clients : ConfigBase
    {
        public static IEnumerable<Client> Get()
        {
            var urls = Configuration["Urls"].Split(';');
            var redirectUris = urls.Select(p => p + "/signin-oidc").ToList();
            var postLogoutRedirectUris = urls.ToList();

            return new List<Client>
            {
                new Client
                {
                    ClientId = "openIdConnectClient",
                    ClientName = "Open Id Connect Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Email.Name,
                        StandardScopes.Roles.Name,
                        "AwesomeAPI"
                    },
                    RedirectUris = redirectUris,
                    PostLogoutRedirectUris = postLogoutRedirectUris
                }
            };
        }
    }
}