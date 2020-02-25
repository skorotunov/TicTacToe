using MediatR;

namespace TicTacToe.Application.NoughtPlayerGameTiles.Commands.CreateNoughtPlayerGameTile
{
    public class CreateNoughtPlayerGameTileCommand : IRequest<int>
    {
        public int GameId { get; set; }

        public byte X { get; set; }

        public byte Y { get; set; }
    }
}
