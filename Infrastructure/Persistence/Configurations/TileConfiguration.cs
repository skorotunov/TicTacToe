using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Infrastructure.Persistence.Configurations
{
    public class TileConfiguration : IEntityTypeConfiguration<Tile>
    {
        public void Configure(EntityTypeBuilder<Tile> builder)
        {
            builder.HasData(
                new Tile { Id = 1, X = 0, Y = 0 },
                new Tile { Id = 2, X = 0, Y = 1 },
                new Tile { Id = 3, X = 0, Y = 2 },
                new Tile { Id = 4, X = 1, Y = 0 },
                new Tile { Id = 5, X = 1, Y = 1 },
                new Tile { Id = 6, X = 1, Y = 2 },
                new Tile { Id = 7, X = 2, Y = 0 },
                new Tile { Id = 8, X = 2, Y = 1 },
                new Tile { Id = 9, X = 2, Y = 2 });
        }
    }
}
