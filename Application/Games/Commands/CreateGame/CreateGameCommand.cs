using MediatR;

namespace TicTacToe.Application.Games.Commands.CreateGame
{
    public class CreateGameCommand : IRequest<int>
    {
        public string OpponentId { get; set; }

        public bool IsOpponentCrossPlayer { get; set; }
    }
}
