using MediatR;

namespace TicTacToe.Application.CrossPlayerGameTiles.Commands.CreateCrossPlayerGameTile
{
    /// <summary>
    /// Command parameters which are necessary to register player X's move.
    /// </summary>
    public class CreateCrossPlayerGameTileCommand : IRequest<int>
    {
        /// <summary>
        /// ID of the game to register move.
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// X coordinate of the tile to register move.
        /// </summary>
        public byte X { get; set; }

        /// <summary>
        /// Y coordinate of the tile to register move.
        /// </summary>
        public byte Y { get; set; }
    }
}
