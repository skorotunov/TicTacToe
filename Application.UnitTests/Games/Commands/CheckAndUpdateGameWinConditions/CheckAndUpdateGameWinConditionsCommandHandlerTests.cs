using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Exceptions;
using TicTacToe.Application.Games.Commands.CheckAndUpdateGameWinConditions;
using TicTacToe.Domain.Enums;
using Xunit;

namespace TicTacToe.Application.UnitTests.Games.Commands.CheckAndUpdateGameWinConditions
{
    public class CheckAndUpdateGameWinConditionsCommandHandlerTests : TestBase
    {
        [Fact]
        public async Task Handle_WhenGameWithTurnNumberLessThanMinimal_ShouldOnlyIncrementTurnNumber()
        {
            // arrange
            int gameId = 5;
            var command = new CheckAndUpdateGameWinConditionsCommand(gameId);
            var handler = new CheckAndUpdateGameWinConditionsCommandHandler(Context);

            // act
            await handler.Handle(command, CancellationToken.None);
            Domain.Entities.Game entity = Context.Games.Find(command.GameId);

            // assert
            entity.ShouldNotBeNull();
            entity.TurnNumber.ShouldBe((byte)4);
        }

        [Fact]
        public void Handle_WhenAppropriateGameDoesNotExist_ShouldThrowNotFoundException()
        {
            // arrange
            int gameId = 999;
            var command = new CheckAndUpdateGameWinConditionsCommand(gameId);
            var handler = new CheckAndUpdateGameWinConditionsCommandHandler(Context);

            // act
            // assert
            Should.ThrowAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenCrossPlayerHasWinConditionTiles_ShouldReturnWin()
        {
            // arrange
            int gameId = 9;
            var command = new CheckAndUpdateGameWinConditionsCommand(gameId);
            var handler = new CheckAndUpdateGameWinConditionsCommandHandler(Context);

            // act
            GameResult result = await handler.Handle(command, CancellationToken.None);
            Domain.Entities.Game entity = Context.Games.Find(command.GameId);

            // assert
            entity.TurnNumber.ShouldBe((byte)5);
            result.ShouldBe(GameResult.Win);
            entity.Result.ShouldBe(GameResult.Win);
        }

        [Fact]
        public async Task Handle_WhenNoughtPlayerHasWinConditionTiles_ShouldReturnLoss()
        {
            // arrange
            int gameId = 8;
            var command = new CheckAndUpdateGameWinConditionsCommand(gameId);
            var handler = new CheckAndUpdateGameWinConditionsCommandHandler(Context);

            // act
            GameResult result = await handler.Handle(command, CancellationToken.None);
            Domain.Entities.Game entity = Context.Games.Find(command.GameId);

            // assert
            entity.TurnNumber.ShouldBe((byte)7);
            result.ShouldBe(GameResult.Loss);
            entity.Result.ShouldBe(GameResult.Loss);
        }

        [Fact]
        public async Task Handle_WhenNoneOfThePlayersHaveWinConditionTilesButThereArePossibility_ShouldReturnActive()
        {
            // arrange
            int gameId = 7;
            var command = new CheckAndUpdateGameWinConditionsCommand(gameId);
            var handler = new CheckAndUpdateGameWinConditionsCommandHandler(Context);

            // act
            GameResult result = await handler.Handle(command, CancellationToken.None);
            Domain.Entities.Game entity = Context.Games.Find(command.GameId);

            // assert
            entity.TurnNumber.ShouldBe((byte)6);
            result.ShouldBe(GameResult.Active);
            entity.Result.ShouldBe(GameResult.Active);
        }

        [Fact]
        public async Task Handle_WhenNoneOfThePlayersHaveWinConditionTilesAndThereAreNoPossibility_ShouldReturnDraw()
        {
            // arrange
            int gameId = 10;
            var command = new CheckAndUpdateGameWinConditionsCommand(gameId);
            var handler = new CheckAndUpdateGameWinConditionsCommandHandler(Context);

            // act
            GameResult result = await handler.Handle(command, CancellationToken.None);
            Domain.Entities.Game entity = Context.Games.Find(command.GameId);

            // assert
            entity.TurnNumber.ShouldBe((byte)8);
            result.ShouldBe(GameResult.Draw);
            entity.Result.ShouldBe(GameResult.Draw);
        }
    }
}
