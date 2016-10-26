using System.Linq;
using System.Threading.Tasks;
using AwesomeServer.Configuration;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AwesomeServer.DatabaseContext
{
    public class SeedDbData
    {
        readonly PersistedGrantDbContext _persistedGrantDbContext;
        readonly ConfigurationDbContext _configurationDbContext;
        readonly UserManager<IdentityUser> _userManager;

        public SeedDbData(
            PersistedGrantDbContext persistedGrantDbContext,
            ConfigurationDbContext configurationDbContext,
            UserManager<IdentityUser> userManager)
        {
            _persistedGrantDbContext = persistedGrantDbContext;
            _configurationDbContext = configurationDbContext;
            _userManager = userManager;
        }

        public async Task EnsureSeedInitialDataAsync()
        {
            await _persistedGrantDbContext.Database.MigrateAsync();
            await _configurationDbContext.Database.MigrateAsync();

            if (!_configurationDbContext.Clients.Any())
            {
                foreach (var client in Clients.Get())
                {
                    _configurationDbContext.Clients.Add(client.ToEntity());
                }
                await _configurationDbContext.SaveChangesAsync();
            }

            if (!_configurationDbContext.Scopes.Any())
            {
                foreach (var scope in Scopes.Get())
                {
                    _configurationDbContext.Scopes.Add(scope.ToEntity());
                }
                await _configurationDbContext.SaveChangesAsync();
            }

            if (!_userManager.Users.Any())
            {
                foreach (var user in Users.Get())
                {
                    var identityUser = new IdentityUser()
                    {
                        Id = user.Subject,
                        UserName = user.Username,
                        Email = user.Username,
                        EmailConfirmed = true
                    };

                    foreach (var claim in user.Claims)
                    {
                        identityUser.Claims.Add(new IdentityUserClaim<string>
                        {
                            UserId = identityUser.Id,
                            ClaimType = claim.Type,
                            ClaimValue = claim.Value,
                        });
                    }

                    await _userManager.CreateAsync(identityUser, user.Password);
                }
            }
        }
    }
}