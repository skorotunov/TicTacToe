using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Interfaces;

namespace TicTacToe.Application.Players.Queries.GetPlayerGames
{
    public class GetPlayerGamesQuery : IRequest<PlayerGamesVM>
    {
        public GetPlayerGamesQuery(string playerId, string currentPlayerId)
        {
            PlayerId = playerId;
            CurrentPlayerId = currentPlayerId;
        }

        public string PlayerId { get; set; }

        public string CurrentPlayerId { get; set; }

        public class GetPlayerGamesQueryHandler : IRequestHandler<GetPlayerGamesQuery, PlayerGamesVM>
        {
            private readonly ITicTacToeDbContext context;
            private readonly IMapper mapper;

            public GetPlayerGamesQueryHandler(ITicTacToeDbContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<PlayerGamesVM> Handle(GetPlayerGamesQuery request, CancellationToken cancellationToken)
            {
                return new PlayerGamesVM
                {
                    Games = await context.Games
                        .Where(x => (x.CrossPlayerId == request.PlayerId || x.NoughtPlayerId == request.PlayerId)
                                 && (x.CrossPlayerId == request.CurrentPlayerId || x.NoughtPlayerId == request.CurrentPlayerId))
                        .OrderByDescending(x => x.StartDate)
                        .ProjectTo<GameDTO>(mapper.ConfigurationProvider, new { currentUserId = request.CurrentPlayerId })
                        .ToListAsync(cancellationToken)
                };
            }
        }
    }
}
