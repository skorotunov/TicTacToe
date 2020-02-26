using TicTacToe.Application.Common.Mappings;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Games.Queries.GetGameTiles
{
    /// <summary>
    /// X's player tiles DTO which represents tiles placed by X player.
    /// </summary>
    public class CrossPlayerTileDTO : IMapFrom<CrossPlayerGameTile>
    {
        public TileDTO Tile { get; set; }
    }
}
