using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using TicTacToe.Domain.Entities;
using TicTacToe.Infrastructure.Identity;

namespace TicTacToe.Infrastructure.Persistence
{
    public class TicTacToeDbContext : ApiAuthorizationDbContext<TicTacToeUser>
    {
        public TicTacToeDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Game> Games { get; set; }

        public DbSet<GameTile> GameTiles { get; set; }

        public DbSet<GameWinCondition> GameWinConditions { get; set; }

        public DbSet<Tile> Tiles { get; set; }

        public DbSet<WinCondition> WinConditions { get; set; }

        public DbSet<WinConditionTile> WinConditionTiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // discover and apply all configurations in current assembly
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
