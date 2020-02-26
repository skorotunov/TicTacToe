using MediatR;

namespace TicTacToe.Application.Games.Queries.GetGameTiles
{
    /// <summary>
    /// Input data to the get game tiles query.
    /// </summary>
    public class GetGameTilesQuery : IRequest<GameTilesVM>
    {
        public GetGameTilesQuery(int gameId)
        {
            GameId = gameId;
        }

        public int GameId { get; set; }
    }
}
