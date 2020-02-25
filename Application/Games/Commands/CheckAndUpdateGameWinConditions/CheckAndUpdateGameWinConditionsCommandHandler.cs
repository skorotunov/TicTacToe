using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Exceptions;
using TicTacToe.Application.Common.Interfaces;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Enums;

namespace TicTacToe.Application.Games.Commands.CheckAndUpdateGameWinConditions
{
    public class CheckAndUpdateGameWinConditionsCommandHandler : IRequestHandler<CheckAndUpdateGameWinConditionsCommand, GameResult>
    {
        // minimum total turns in the game required for possible win
        private const int MinimumNumerOfTurnsToWin = 5;

        // total number of tiles to fiil by one player to win
        private const int WinningNumberOfTiles = 3;

        private readonly ITicTacToeDbContext context;

        public CheckAndUpdateGameWinConditionsCommandHandler(ITicTacToeDbContext context)
        {
            this.context = context;
        }

        public async Task<GameResult> Handle(CheckAndUpdateGameWinConditionsCommand request, CancellationToken cancellationToken)
        {
            Game game = await context.Games
                .Include(x => x.CrossPlayerGameTiles)
                .Include(x => x.NoughtPlayerGameTiles)
                .FirstOrDefaultAsync(X => X.Id == request.GameId);
            if (game == null)
            {
                throw new NotFoundException(nameof(Game), request.GameId);
            }

            // check win conditions of the players only when win is possible. Increment turn number property
            if (++game.TurnNumber >= MinimumNumerOfTurnsToWin)
            {
                List<WinCondition> winConditions = await context.WinConditions
                    .Include(x => x.WinConditionTiles)
                    .AsNoTracking()
                    .ToListAsync();
                int notReachableWinConditionsCount = 0;
                foreach (WinCondition winCondition in winConditions)
                {
                    int crossPlayerWinConditionTilesCount = 0, noughtPlayerWinConditionTilesCount = 0;
                    foreach (WinConditionTile winConditionTile in winCondition.WinConditionTiles)
                    {
                        if (game.CrossPlayerGameTiles.Any(x => x.TileId == winConditionTile.TileId))
                        {
                            crossPlayerWinConditionTilesCount++;
                        }

                        if (game.NoughtPlayerGameTiles.Any(x => x.TileId == winConditionTile.TileId))
                        {
                            noughtPlayerWinConditionTilesCount++;
                        }

                        // check that win condition is not reachable in this game (both player have filled tiles of one condition)
                        if (crossPlayerWinConditionTilesCount != 0 && noughtPlayerWinConditionTilesCount != 0)
                        {
                            notReachableWinConditionsCount++;
                            break;
                        }
                    }

                    // check cross player for the win
                    if (crossPlayerWinConditionTilesCount == WinningNumberOfTiles)
                    {
                        game.Result = GameResult.Win;
                        break;
                    }

                    // check nought player for the win
                    if (noughtPlayerWinConditionTilesCount == WinningNumberOfTiles)
                    {
                        game.Result = GameResult.Loss;
                        break;
                    }
                }

                // check for the draw - no win conditions left
                if (winConditions.Count == notReachableWinConditionsCount)
                {
                    game.Result = GameResult.Draw;
                }
            }

            await context.SaveChangesAsync(cancellationToken);
            return game.Result;
        }
    }
}
