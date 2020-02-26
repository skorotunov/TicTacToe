using TicTacToe.Application.Common.Mappings;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Games.Queries.GetGameTiles
{
    public class NoughtPlayerTileDTO : IMapFrom<NoughtPlayerGameTile>
    {
        public TileDTO Tile { get; set; }
    }
}
