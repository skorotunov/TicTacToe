using TicTacToe.Application.Common.Mappings;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Games.Queries.GetGameTiles
{
    public class TileDTO : IMapFrom<Tile>
    {
        public byte X { get; set; }

        public byte Y { get; set; }
    }
}
