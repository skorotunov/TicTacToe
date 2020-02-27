using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Exceptions;
using TicTacToe.Application.CrossPlayerGameTiles.Commands.CreateCrossPlayerGameTile;
using TicTacToe.Domain.Entities;
using Xunit;

namespace TicTacToe.Application.UnitTests.CrossPlayerGameTiles.Commands.CreateCrossPlayerGameTile
{
    public class CreateCrossPlayerGameTileCommandHandlerTests : TestBase
    {
        [Fact]
        public async Task Handle_WhenAppropriateTileExists_ShouldCreateCrossPlayerGameTile()
        {
            // arrange
            var command = new CreateCrossPlayerGameTileCommand
            {
                GameId = 1,
                X = 3,
                Y = 3
            };
            var handler = new CreateCrossPlayerGameTileCommandHandler(Context);

            // act
            int result = await handler.Handle(command, CancellationToken.None);
            CrossPlayerGameTile entity = Context.CrossPlayerGameTiles.Find(result);

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
            var command = new CreateCrossPlayerGameTileCommand
            {
                GameId = 1,
                X = 3,
                Y = 3
            };
            var handler = new CreateCrossPlayerGameTileCommandHandler(Context);

            // act
            // assert
            Should.ThrowAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
