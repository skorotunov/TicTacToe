using TicTacToe.Application.Common.Mappings;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Games.Queries.GetGameTiles
{
    /// <summary>
    /// Tile DTO which represents properties from Tile entity.
    /// </summary>
    public class TileDTO : IMapFrom<Tile>
    {
        public byte X { get; set; }

        public byte Y { get; set; }
    }
}
