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
                new WinConditionTile { WinConditionId = 1, TileId = 1 },
                new WinConditionTile { WinConditionId = 1, TileId = 4 },
                new WinConditionTile { WinConditionId = 1, TileId = 7 },

                // middle horizontal line
                new WinConditionTile { WinConditionId = 2, TileId = 2 },
                new WinConditionTile { WinConditionId = 2, TileId = 5 },
                new WinConditionTile { WinConditionId = 2, TileId = 8 },

                // highest horizontal line
                new WinConditionTile { WinConditionId = 3, TileId = 3 },
                new WinConditionTile { WinConditionId = 3, TileId = 6 },
                new WinConditionTile { WinConditionId = 3, TileId = 9 },

                // left vertical line
                new WinConditionTile { WinConditionId = 4, TileId = 1 },
                new WinConditionTile { WinConditionId = 4, TileId = 2 },
                new WinConditionTile { WinConditionId = 4, TileId = 3 },
                // middle vertical line
                new WinConditionTile { WinConditionId = 5, TileId = 4 },
                new WinConditionTile { WinConditionId = 5, TileId = 5 },
                new WinConditionTile { WinConditionId = 5, TileId = 6 },

                // right vertical line
                new WinConditionTile { WinConditionId = 6, TileId = 7 },
                new WinConditionTile { WinConditionId = 6, TileId = 8 },
                new WinConditionTile { WinConditionId = 6, TileId = 9 },

                // first diagonal
                new WinConditionTile { WinConditionId = 7, TileId = 1 },
                new WinConditionTile { WinConditionId = 7, TileId = 5 },
                new WinConditionTile { WinConditionId = 7, TileId = 9 },

                // second diagonal
                new WinConditionTile { WinConditionId = 8, TileId = 3 },
                new WinConditionTile { WinConditionId = 8, TileId = 5 },
                new WinConditionTile { WinConditionId = 8, TileId = 7 });
        }
    }
}
