using MediatR;

namespace TicTacToe.Application.Players.Queries.GetPlayerGames
{
    /// <summary>
    /// Data which contains in the query parameters.
    /// </summary>
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
