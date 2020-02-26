using TicTacToe.Application.Common.Mappings;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Games.Queries.GetGameTiles
{
    /// <summary>
    /// O's player tiles DTO which represents tiles placed by O player.
    /// </summary>
    public class NoughtPlayerTileDTO : IMapFrom<NoughtPlayerGameTile>
    {
        public TileDTO Tile { get; set; }
    }
}
