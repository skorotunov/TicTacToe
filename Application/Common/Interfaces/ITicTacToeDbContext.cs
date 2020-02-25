using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Common.Interfaces
{
    public interface ITicTacToeDbContext
    {
        DbSet<CrossPlayerGameTile> CrossPlayerGameTiles { get; set; }

        DbSet<Game> Games { get; set; }

        DbSet<NoughtPlayerGameTile> NoughtPlayerGameTiles { get; set; }

        DbSet<Tile> Tiles { get; set; }

        DbSet<WinCondition> WinConditions { get; set; }

        DbSet<WinConditionTile> WinConditionTiles { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
