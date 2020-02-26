using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Exceptions;
using TicTacToe.Application.Common.Interfaces;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Games.Queries.GetGameTiles
{
    public class GetGameTilesQueryHandler : IRequestHandler<GetGameTilesQuery, GameTilesVM>
    {
        private readonly ITicTacToeDbContext context;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;

        public GetGameTilesQueryHandler(ITicTacToeDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            this.context = context;
            this.mapper = mapper;
            this.currentUserService = currentUserService;
        }

        public async Task<GameTilesVM> Handle(GetGameTilesQuery request, CancellationToken cancellationToken)
        {
            Game entity = await context.Games.FindAsync(request.GameId);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Game), request.GameId);
            }

            return new GameTilesVM
            {
                IsOpponentCrossPlayer = currentUserService.UserId == entity.NoughtPlayerId,
                CrossPlayerTiles = await context.CrossPlayerGameTiles
                    .Where(x => x.GameId == request.GameId)
                    .Include(x => x.Tile)
                    .AsNoTracking()
                    .ProjectTo<CrossPlayerTileDTO>(mapper.ConfigurationProvider)
                    .ToListAsync(),
                NoughtPlayerTiles = await context.NoughtPlayerGameTiles
                    .Where(x => x.GameId == request.GameId)
                    .Include(x => x.Tile)
                    .AsNoTracking()
                    .ProjectTo<NoughtPlayerTileDTO>(mapper.ConfigurationProvider)
                    .ToListAsync()
            };
        }
    }
}
