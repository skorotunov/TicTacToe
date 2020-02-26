using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Common.Interfaces
{
    /// <summary>
    /// Service which represents system database context. EF core dbcontext represents Unit of Work.
    /// </summary>
    public interface ITicTacToeDbContext
    {
        /// <summary>
        /// Repository of the CrossPlayerGameTile entities.
        /// </summary>
        DbSet<CrossPlayerGameTile> CrossPlayerGameTiles { get; set; }

        /// <summary>
        /// Repository of the Game entities.
        /// </summary>
        DbSet<Game> Games { get; set; }

        /// <summary>
        /// Repository of the NoughtPlayerGameTile entities.
        /// </summary>
        DbSet<NoughtPlayerGameTile> NoughtPlayerGameTiles { get; set; }

        /// <summary>
        /// Repository of the Tile entities.
        /// </summary>
        DbSet<Tile> Tiles { get; set; }

        /// <summary>
        /// Repository of the WinCondition entities.
        /// </summary>
        DbSet<WinCondition> WinConditions { get; set; }

        /// <summary>
        /// Repository of the WinConditionTile entities.
        /// </summary>
        DbSet<WinConditionTile> WinConditionTiles { get; set; }

        /// <summary>
        /// Commit changes to the databasy in async way.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancell task.</param>
        /// <returns>Result of the database commit.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
