using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Application.Players.Queries.GetPlayerGames;
using Xunit;

namespace TicTacToe.Application.UnitTests.Players.Queries.GetPlayerGames
{
    public class GetPlayerGamesQueryHandlerTests : TestBase
    {
        [Fact]
        public async Task Handle_OnRegularInput_ShouldReturnCorrectNumberOfGames()
        {
            // arrange
            string playerId = "player1";
            string currentPlayerId = "player2";
            var query = new GetPlayerGamesQuery(playerId, currentPlayerId);
            var handler = new GetPlayerGamesQueryHandler(Context, Mapper);

            // act
            PlayerGamesVM result = await handler.Handle(query, CancellationToken.None);

            // assert
            result.ShouldBeOfType<PlayerGamesVM>();
            result.Games.Count.ShouldBe(4);
        }

        [Fact]
        public async Task Handle_OnRegularInput_ShouldReturnCorrectlyOrderedAndMappedGames()
        {
            // arrange
            string playerId = "player1";
            string currentPlayerId = "player2";
            var query = new GetPlayerGamesQuery(playerId, currentPlayerId);
            var handler = new GetPlayerGamesQueryHandler(Context, Mapper);

            // act
            PlayerGamesVM result = await handler.Handle(query, CancellationToken.None);

            // assert
            result.ShouldBeOfType<PlayerGamesVM>();
            result.Games[0].StartDate.ShouldBe(new DateTime(2020, 2, 20).ToString("g"));
        }

        [Fact]
        public async Task Handle_WhenResultIsLossAndCurrentUserIsCrossPlayer_ShouldReturnDTOWithResultLoss()
        {
            // arrange
            string playerId = "player1";
            string currentPlayerId = "player2";
            var query = new GetPlayerGamesQuery(playerId, currentPlayerId);
            var handler = new GetPlayerGamesQueryHandler(Context, Mapper);

            // act
            PlayerGamesVM result = await handler.Handle(query, CancellationToken.None);

            // assert
            result.ShouldBeOfType<PlayerGamesVM>();
            result.Games[0].Result.ShouldBe(Domain.Enums.GameResult.Loss.ToString());
        }

        [Fact]
        public async Task Handle_WhenResultIsLossAndCurrentUserIsNotCrossPlayer_ShouldReturnDTOWithResultWin()
        {
            // arrange
            string playerId = "player2";
            string currentPlayerId = "player1";
            var query = new GetPlayerGamesQuery(playerId, currentPlayerId);
            var handler = new GetPlayerGamesQueryHandler(Context, Mapper);

            // act
            PlayerGamesVM result = await handler.Handle(query, CancellationToken.None);

            // assert
            result.ShouldBeOfType<PlayerGamesVM>();
            result.Games[0].Result.ShouldBe(Domain.Enums.GameResult.Win.ToString());
        }

        [Fact]
        public async Task Handle_WhenResultIsWinAndCurrentUserIsCrossPlayer_ShouldReturnDTOWithResultWin()
        {
            // arrange
            string playerId = "player2";
            string currentPlayerId = "player1";
            var query = new GetPlayerGamesQuery(playerId, currentPlayerId);
            var handler = new GetPlayerGamesQueryHandler(Context, Mapper);

            // act
            PlayerGamesVM result = await handler.Handle(query, CancellationToken.None);

            // assert
            result.ShouldBeOfType<PlayerGamesVM>();
            result.Games[1].Result.ShouldBe(Domain.Enums.GameResult.Win.ToString());
        }

        [Fact]
        public async Task Handle_WhenResultIsWinAndCurrentUserIsNotCrossPlayer_ShouldReturnDTOWithResultLoss()
        {
            // arrange
            string playerId = "player1";
            string currentPlayerId = "player2";
            var query = new GetPlayerGamesQuery(playerId, currentPlayerId);
            var handler = new GetPlayerGamesQueryHandler(Context, Mapper);

            // act
            PlayerGamesVM result = await handler.Handle(query, CancellationToken.None);

            // assert
            result.ShouldBeOfType<PlayerGamesVM>();
            result.Games[1].Result.ShouldBe(Domain.Enums.GameResult.Loss.ToString());
        }

        [Fact]
        public async Task Handle_WhenResultIsDraw_ShouldReturnDTOWithResultDraw()
        {
            // arrange
            string playerId = "player1";
            string currentPlayerId = "player2";
            var query = new GetPlayerGamesQuery(playerId, currentPlayerId);
            var handler = new GetPlayerGamesQueryHandler(Context, Mapper);

            // act
            PlayerGamesVM result = await handler.Handle(query, CancellationToken.None);

            // assert
            result.ShouldBeOfType<PlayerGamesVM>();
            result.Games[2].Result.ShouldBe(Domain.Enums.GameResult.Draw.ToString());
        }

        [Fact]
        public async Task Handle_WhenResultIsActive_ShouldReturnDTOWithResultActive()
        {
            // arrange
            string playerId = "player1";
            string currentPlayerId = "player2";
            var query = new GetPlayerGamesQuery(playerId, currentPlayerId);
            var handler = new GetPlayerGamesQueryHandler(Context, Mapper);

            // act
            PlayerGamesVM result = await handler.Handle(query, CancellationToken.None);

            // assert
            result.ShouldBeOfType<PlayerGamesVM>();
            result.Games[3].Result.ShouldBe(Domain.Enums.GameResult.Active.ToString());
        }
    }
}
