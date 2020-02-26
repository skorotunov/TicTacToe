using MediatR;
using TicTacToe.Domain.Enums;

namespace TicTacToe.Application.Games.Commands.CheckAndUpdateGameWinConditions
{
    /// <summary>
    /// Command input parameters.
    /// </summary>
    public class CheckAndUpdateGameWinConditionsCommand : IRequest<GameResult>
    {
        public CheckAndUpdateGameWinConditionsCommand(int gameId)
        {
            GameId = gameId;
        }

        public int GameId { get; set; }
    }
}
