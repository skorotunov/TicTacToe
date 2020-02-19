using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Infrastructure.Persistence.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            // Restrict length of the string user keyes accordingly to IdentityUser configuration
            builder.Property(x => x.CrossPlayerId)
                .HasMaxLength(450)
                .IsRequired();
            builder.Property(x => x.NoughtPlayerId)
                .HasMaxLength(450)
                .IsRequired();
        }
    }
}
