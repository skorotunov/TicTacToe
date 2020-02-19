using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Infrastructure.Persistence.Configurations
{
    public class WinConditionTileConfiguration : IEntityTypeConfiguration<WinConditionTile>
    {
        public void Configure(EntityTypeBuilder<WinConditionTile> builder)
        {
            builder.HasData(

                // lowest horizontal line
                new WinConditionTile { Id = 1, WinConditionId = 1, TileId = 1 },
                new WinConditionTile { Id = 2, WinConditionId = 1, TileId = 4 },
                new WinConditionTile { Id = 3, WinConditionId = 1, TileId = 7 },

                // middle horizontal line
                new WinConditionTile { Id = 4, WinConditionId = 2, TileId = 2 },
                new WinConditionTile { Id = 5, WinConditionId = 2, TileId = 5 },
                new WinConditionTile { Id = 6, WinConditionId = 2, TileId = 8 },

                // highest horizontal line
                new WinConditionTile { Id = 7, WinConditionId = 3, TileId = 3 },
                new WinConditionTile { Id = 8, WinConditionId = 3, TileId = 6 },
                new WinConditionTile { Id = 9, WinConditionId = 3, TileId = 9 },

                // left vertical line
                new WinConditionTile { Id = 10, WinConditionId = 4, TileId = 1 },
                new WinConditionTile { Id = 11, WinConditionId = 4, TileId = 2 },
                new WinConditionTile { Id = 12, WinConditionId = 4, TileId = 3 },
                // middle vertical line
                new WinConditionTile { Id = 13, WinConditionId = 5, TileId = 4 },
                new WinConditionTile { Id = 14, WinConditionId = 5, TileId = 5 },
                new WinConditionTile { Id = 15, WinConditionId = 5, TileId = 6 },

                // right vertical line
                new WinConditionTile { Id = 16, WinConditionId = 6, TileId = 7 },
                new WinConditionTile { Id = 17, WinConditionId = 6, TileId = 8 },
                new WinConditionTile { Id = 18, WinConditionId = 6, TileId = 9 },

                // first diagonal
                new WinConditionTile { Id = 19, WinConditionId = 7, TileId = 1 },
                new WinConditionTile { Id = 20, WinConditionId = 7, TileId = 5 },
                new WinConditionTile { Id = 21, WinConditionId = 7, TileId = 9 },

                // second diagonal
                new WinConditionTile { Id = 22, WinConditionId = 8, TileId = 3 },
                new WinConditionTile { Id = 23, WinConditionId = 8, TileId = 5 },
                new WinConditionTile { Id = 24, WinConditionId = 8, TileId = 7 });
        }
    }
}
