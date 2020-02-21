using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TicTacToe.Domain.Entities;
using TicTacToe.Infrastructure.Identity;

namespace TicTacToe.Infrastructure.Persistence
{
    public class TicTacToeDbContext : IdentityDbContext<TicTacToeUser>
    {
        public TicTacToeDbContext(DbContextOptions<TicTacToeDbContext> options)
            : base(options)
        {
        }

        public DbSet<CrossPlayerGameTile> CrossPlayerGameTiles { get; set; }

        public DbSet<CrossPlayerGameWinCondition> CrossPlayerGameWinConditions { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<NoughtPlayerGameTile> NoughtPlayerGameTiles { get; set; }

        public DbSet<NoughtPlayerGameWinCondition> NoughtPlayerGameWinConditions { get; set; }

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
