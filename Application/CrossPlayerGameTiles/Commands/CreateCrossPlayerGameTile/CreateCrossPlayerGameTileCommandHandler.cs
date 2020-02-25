﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Exceptions;
using TicTacToe.Application.Common.Interfaces;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.CrossPlayerGameTiles.Commands.CreateCrossPlayerGameTile
{
    public class CreateCrossPlayerGameTileCommandHandler : IRequestHandler<CreateCrossPlayerGameTileCommand, int>
    {
        private readonly ITicTacToeDbContext context;

        public CreateCrossPlayerGameTileCommandHandler(ITicTacToeDbContext context)
        {
            this.context = context;
        }

        public async Task<int> Handle(CreateCrossPlayerGameTileCommand request, CancellationToken cancellationToken)
        {
            Tile tileEntity = await context.Tiles.AsQueryable().FirstOrDefaultAsync(x => x.X == request.X && x.Y == request.Y);
            if (tileEntity == null)
            {
                throw new NotFoundException(nameof(Tile), null);
            }

            var entity = new CrossPlayerGameTile
            {
                GameId = request.GameId,
                TileId = tileEntity.Id
            };
            context.CrossPlayerGameTiles.Add(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}