using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Exceptions;
using TicTacToe.Application.Common.Interfaces;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.NoughtPlayerGameTiles.Commands.CreateNoughtPlayerGameTile
{
    /// <summary>
    /// Handler which inserts data about the move to the database.
    /// </summary>
    public class CreateNoughtPlayerGameTileCommandHandler : IRequestHandler<CreateNoughtPlayerGameTileCommand, int>
    {
        private readonly ITicTacToeDbContext context;

        public CreateNoughtPlayerGameTileCommandHandler(ITicTacToeDbContext context)
        {
            this.context = context;
        }

        public async Task<int> Handle(CreateNoughtPlayerGameTileCommand request, CancellationToken cancellationToken)
        {
            Tile tileEntity = await context.Tiles.AsNoTracking().FirstOrDefaultAsync(x => x.X == request.X && x.Y == request.Y);
            if (tileEntity == null)
            {
                throw new NotFoundException(nameof(Tile), null);
            }

            var entity = new NoughtPlayerGameTile
            {
                GameId = request.GameId,
                TileId = tileEntity.Id
            };
            context.NoughtPlayerGameTiles.Add(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}
