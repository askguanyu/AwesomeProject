using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;

namespace AwesomeServer.Configuration
{
    internal class Scopes : ConfigBase
    {
        public static IEnumerable<Scope> Get()
        {
            return new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.Email,
                StandardScopes.Roles,
                StandardScopes.OfflineAccess,
                new Scope
                {
                    Name = "AwesomeAPI",
                    DisplayName = "Awesome API",
                    Description = "Awesome API Scope",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim(JwtClaimTypes.Role)
                    },
                    ScopeSecrets = new List<Secret>
                    {
                        new Secret(Configuration["Secret"].Sha256())
                    }
                }
            };
        }
    }
}