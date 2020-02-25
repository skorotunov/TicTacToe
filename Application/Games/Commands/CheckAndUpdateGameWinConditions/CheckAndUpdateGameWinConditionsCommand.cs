using MediatR;
using TicTacToe.Domain.Enums;

namespace TicTacToe.Application.Games.Commands.CheckAndUpdateGameWinConditions
{
    public class CheckAndUpdateGameWinConditionsCommand : IRequest<GameResult>
    {
        public CheckAndUpdateGameWinConditionsCommand(int gameId)
        {
            GameId = gameId;
        }

        public int GameId { get; set; }
    }
}
