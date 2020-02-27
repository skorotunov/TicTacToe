using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Application.Games.Commands.CreateGame;
using TicTacToe.Domain.Entities;
using Xunit;

namespace TicTacToe.Application.UnitTests.Games.Commands.CreateGame
{
    public class CreateGameCommandHandlerTests : TestBase
    {
        [Fact]
        public async Task Handle_WhenOpponentIsCrossPlayer_ShouldCreateGameWithCrossPlayerAsOpponent()
        {
            // arrange
            string opponentId = "opponentId";
            bool isOpponentCrossPlayer = true;
            var command = new CreateGameCommand
            {
                OpponentId = opponentId,
                IsOpponentCrossPlayer = isOpponentCrossPlayer
            };
            var handler = new CreateGameCommandHandler(Context, CurrentUserService, DateTime);

            // act
            int result = await handler.Handle(command, CancellationToken.None);
            Game entity = Context.Games.Find(result);

            // assert
            entity.ShouldNotBeNull();
            entity.StartDate.ShouldBe(DateTime.Now);
            entity.CrossPlayerId.ShouldBe(opponentId);
            entity.NoughtPlayerId.ShouldBe(CurrentUserService.UserId);
        }

        [Fact]
        public async Task Handle_WhenOpponentIsNotCrossPlayer_ShouldCreateGameWithCrossPlayerAsCurrentUser()
        {
            // arrange
            string opponentId = "opponentId";
            bool isOpponentCrossPlayer = false;
            var command = new CreateGameCommand
            {
                OpponentId = opponentId,
                IsOpponentCrossPlayer = isOpponentCrossPlayer
            };
            var handler = new CreateGameCommandHandler(Context, CurrentUserService, DateTime);

            // act
            int result = await handler.Handle(command, CancellationToken.None);
            Game entity = Context.Games.Find(result);

            // assert
            entity.ShouldNotBeNull();
            entity.StartDate.ShouldBe(DateTime.Now);
            entity.CrossPlayerId.ShouldBe(CurrentUserService.UserId);
            entity.NoughtPlayerId.ShouldBe(opponentId);
        }
    }
}
