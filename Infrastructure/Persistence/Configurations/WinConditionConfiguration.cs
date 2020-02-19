using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Infrastructure.Persistence.Configurations
{
    public class WinConditionConfiguration : IEntityTypeConfiguration<WinCondition>
    {
        public void Configure(EntityTypeBuilder<WinCondition> builder)
        {
            builder.HasData(
                new WinCondition { Id = 1 },
                new WinCondition { Id = 2 },
                new WinCondition { Id = 3 },
                new WinCondition { Id = 4 },
                new WinCondition { Id = 5 },
                new WinCondition { Id = 6 },
                new WinCondition { Id = 7 },
                new WinCondition { Id = 8 });
        }
    }
}
