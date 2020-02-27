using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Exceptions;
using TicTacToe.Application.NoughtPlayerGameTiles.Commands.CreateNoughtPlayerGameTile;
using TicTacToe.Domain.Entities;
using Xunit;

namespace TicTacToe.Application.UnitTests.NoughtPlayerGameTiles.Commands.CreateNoughtPlayerGameTile
{
    public class CreateNoughtPlayerGameTileCommandHandlerTests : TestBase
    {
        [Fact]
        public async Task Handle_WhenAppropriateTileExists_ShouldCreateNoughtPlayerGameTile()
        {
            // arrange
            var command = new CreateNoughtPlayerGameTileCommand
            {
                GameId = 1,
                X = 3,
                Y = 3
            };
            var handler = new CreateNoughtPlayerGameTileCommandHandler(Context);

            // act
            int result = await handler.Handle(command, CancellationToken.None);
            NoughtPlayerGameTile entity = Context.NoughtPlayerGameTiles.Find(result);

            // assert
            entity.ShouldNotBeNull();
            entity.Game.Id.ShouldBe(command.GameId);
            ((int)entity.Tile.X).ShouldBe(command.X);
            ((int)entity.Tile.Y).ShouldBe(command.Y);
        }

        [Fact]
        public void Handle_WhenAppropriateTileDoesNotExist_ShouldThrowNotFoundException()
        {
            // arrange
            var command = new CreateNoughtPlayerGameTileCommand
            {
                GameId = 1,
                X = 3,
                Y = 3
            };
            var handler = new CreateNoughtPlayerGameTileCommandHandler(Context);

            // act
            // assert
            Should.ThrowAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
