using AwesomeServer.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AwesomeServer.DatabaseContext
{
    public class ServerDbContext : IdentityDbContext<IdentityUser>
    {
        readonly IOptions<ServerOptions> _serverOptions;

        public ServerDbContext(
            DbContextOptions<ServerDbContext> options,
            IOptions<ServerOptions> serverOptions)
            : base(options)
        {
            _serverOptions = serverOptions;
            Database.MigrateAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_serverOptions.Value.DbConnection);
            base.OnConfiguring(optionsBuilder);
        }
    }
}