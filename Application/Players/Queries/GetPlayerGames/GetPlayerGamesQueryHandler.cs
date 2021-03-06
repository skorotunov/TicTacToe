﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Interfaces;

namespace TicTacToe.Application.Players.Queries.GetPlayerGames
{
    /// <summary>
    /// Handler of the query. Gets incoming data and outputs desired view model.
    /// </summary>
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
                // get all games between two players
                Games = await context.Games
                    .Where(x => (x.CrossPlayerId == request.PlayerId || x.NoughtPlayerId == request.PlayerId)
                             && (x.CrossPlayerId == request.CurrentPlayerId || x.NoughtPlayerId == request.CurrentPlayerId))
                    .OrderByDescending(x => x.StartDate)
                    .AsNoTracking()
                    .ProjectTo<GameDTO>(mapper.ConfigurationProvider, new { currentUserId = request.CurrentPlayerId })
                    .ToListAsync(cancellationToken)
            };
        }
    }
}
