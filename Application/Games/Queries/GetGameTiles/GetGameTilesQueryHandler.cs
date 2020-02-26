using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Interfaces;

namespace TicTacToe.Application.Games.Queries.GetGameTiles
{
    public class GetGameTilesQueryHandler : IRequestHandler<GetGameTilesQuery, GameTilesVM>
    {
        private readonly ITicTacToeDbContext context;
        private readonly IMapper mapper;

        public GetGameTilesQueryHandler(ITicTacToeDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<GameTilesVM> Handle(GetGameTilesQuery request, CancellationToken cancellationToken)
        {
            return new GameTilesVM
            {
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
