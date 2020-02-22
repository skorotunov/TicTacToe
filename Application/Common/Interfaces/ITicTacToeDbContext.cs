using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Common.Interfaces
{
    public interface ITicTacToeDbContext
    {
        DbSet<Game> Games { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
