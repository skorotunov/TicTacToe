using MediatR;

namespace TicTacToe.Application.Games.Queries.GetGameTiles
{
    public class GetGameTilesQuery : IRequest<GameTilesVM>
    {
        public GetGameTilesQuery(int gameId)
        {
            GameId = gameId;
        }

        public int GameId { get; set; }
    }
}
