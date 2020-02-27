using Shouldly;
using TicTacToe.Application.Players.Queries.GetPlayerGames;
using Xunit;

namespace TicTacToe.Application.UnitTests.Players.Queries.GetPlayerGames
{
    public class GetPlayerGamesQueryValidatorTests : TestBase
    {
        [Fact]
        public void IsValid_WhenPlayerIdsAreNotUnique_ShouldBeFalse()
        {
            // arrange
            string playerId = "player1";
            string currentPlayerId = "player1";
            var query = new GetPlayerGamesQuery(playerId, currentPlayerId);
            var validator = new GetPlayerGamesQueryValidator();

            // act
            FluentValidation.Results.ValidationResult result = validator.Validate(query);

            // assert
            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_WhenPlayerIdIsEmpty_ShouldBeFalse()
        {
            // arrange
            string playerId = string.Empty;
            string currentPlayerId = "player1";
            var query = new GetPlayerGamesQuery(playerId, currentPlayerId);
            var validator = new GetPlayerGamesQueryValidator();

            // act
            FluentValidation.Results.ValidationResult result = validator.Validate(query);

            // assert
            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_WhenPlayerIdIsNull_ShouldBeFalse()
        {
            // arrange
            string playerId = null;
            string currentPlayerId = "player1";
            var query = new GetPlayerGamesQuery(playerId, currentPlayerId);
            var validator = new GetPlayerGamesQueryValidator();

            // act
            FluentValidation.Results.ValidationResult result = validator.Validate(query);

            // assert
            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_WhenCurrentPlayerIdIsEmpty_ShouldBeFalse()
        {
            // arrange
            string playerId = "player1";
            string currentPlayerId = string.Empty;
            var query = new GetPlayerGamesQuery(playerId, currentPlayerId);
            var validator = new GetPlayerGamesQueryValidator();

            // act
            FluentValidation.Results.ValidationResult result = validator.Validate(query);

            // assert
            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_WhenCurrentPlayerIdIsNull_ShouldBeFalse()
        {
            // arrange
            string playerId = "player1";
            string currentPlayerId = null;
            var query = new GetPlayerGamesQuery(playerId, currentPlayerId);
            var validator = new GetPlayerGamesQueryValidator();

            // act
            FluentValidation.Results.ValidationResult result = validator.Validate(query);

            // assert
            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_WhenPlayerIdsAreUniqueAndAreNotEmptyOrNull_ShouldBeTrue()
        {
            // arrange
            string playerId = "player1";
            string currentPlayerId = "player2";
            var query = new GetPlayerGamesQuery(playerId, currentPlayerId);
            var validator = new GetPlayerGamesQueryValidator();

            // act
            FluentValidation.Results.ValidationResult result = validator.Validate(query);

            // assert
            result.IsValid.ShouldBe(true);
        }
    }
}
