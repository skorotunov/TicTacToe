using MediatR;

namespace TicTacToe.Application.Games.Commands.CreateGame
{
    /// <summary>
    /// Command input parameters.
    /// </summary>
    public class CreateGameCommand : IRequest<int>
    {
        public string OpponentId { get; set; }

        public bool IsOpponentCrossPlayer { get; set; }
    }
}
