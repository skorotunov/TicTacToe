using MediatR;

namespace TicTacToe.Application.NoughtPlayerGameTiles.Commands.CreateNoughtPlayerGameTile
{
    /// <summary>
    /// Command parameters which are necessary to register player O's move.
    /// </summary>
    public class CreateNoughtPlayerGameTileCommand : IRequest<int>
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
