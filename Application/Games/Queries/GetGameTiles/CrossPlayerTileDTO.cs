using TicTacToe.Application.Common.Mappings;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Games.Queries.GetGameTiles
{
    public class CrossPlayerTileDTO : IMapFrom<CrossPlayerGameTile>
    {
        public TileDTO Tile { get; set; }
    }
}
