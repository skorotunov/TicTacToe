using MediatR;

namespace TicTacToe.Application.CrossPlayerGameTiles.Commands.CreateCrossPlayerGameTile
{
    public class CreateCrossPlayerGameTileCommand : IRequest<int>
    {
        public int GameId { get; set; }

        public byte X { get; set; }

        public byte Y { get; set; }
    }
}
