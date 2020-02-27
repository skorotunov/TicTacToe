using Shouldly;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Exceptions;
using TicTacToe.Application.Games.Queries.GetGameTiles;
using Xunit;

namespace TicTacToe.Application.UnitTests.Games.Queries
{
    public class GetGameTilesQueryHandlerTests : TestBase
    {
        [Fact]
        public void Handle_WhenAppropriateGameDoesNotExist_ShouldThrowNotFoundException()
        {
            // arrange
            int gameId = 999;
            var query = new GetGameTilesQuery(gameId);
            var handler = new GetGameTilesQueryHandler(Context, Mapper, CurrentUserService);

            // act
            // assert
            Should.ThrowAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenCurrentUserIsCrossPlayer_ShouldReturnIsOpponentCrossPlayerFalse()
        {
            // arrange
            int gameId = 7;
            var query = new GetGameTilesQuery(gameId);
            var handler = new GetGameTilesQueryHandler(Context, Mapper, CurrentUserService);

            // act
            GameTilesVM result = await handler.Handle(query, CancellationToken.None);

            // assert
            result.ShouldBeOfType<GameTilesVM>();
            result.IsOpponentCrossPlayer.ShouldBe(false);
        }

        [Fact]
        public async Task Handle_OnRegularInput_ShouldReturnCorrectNumberAndTypeOfPlayerTiles()
        {
            // arrange
            int gameId = 1;
            var query = new GetGameTilesQuery(gameId);
            var handler = new GetGameTilesQueryHandler(Context, Mapper, CurrentUserService);

            // act
            GameTilesVM result = await handler.Handle(query, CancellationToken.None);

            // assert
            result.CrossPlayerTiles.First().ShouldBeOfType<CrossPlayerTileDTO>();
            result.NoughtPlayerTiles.First().ShouldBeOfType<NoughtPlayerTileDTO>();
            result.CrossPlayerTiles.Count.ShouldBe(2);
            result.NoughtPlayerTiles.Count.ShouldBe(1);
        }
    }
}
