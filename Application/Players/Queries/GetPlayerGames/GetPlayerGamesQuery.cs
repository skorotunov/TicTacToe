using MediatR;

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
    }
}
