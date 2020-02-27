using Shouldly;
using TicTacToe.Application.Games.Commands.CheckAndUpdateGameWinConditions;
using Xunit;

namespace TicTacToe.Application.UnitTests.Games.Commands.CheckAndUpdateGameWinConditions
{
    public class CheckAndUpdateGameWinConditionsCommandValidatorTests : TestBase
    {
        [Fact]
        public void IsValid_WhenGameIdIsNought_ShouldBeFalse()
        {
            // arrange
            int gameId = 0;
            var command = new CheckAndUpdateGameWinConditionsCommand(gameId);
            var validator = new CheckAndUpdateGameWinConditionsCommandValidator();

            // act
            FluentValidation.Results.ValidationResult result = validator.Validate(command);

            // assert
            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_WhenGameIdIsNegative_ShouldBeFalse()
        {
            // arrange
            int gameId = -10;
            var command = new CheckAndUpdateGameWinConditionsCommand(gameId);
            var validator = new CheckAndUpdateGameWinConditionsCommandValidator();

            // act
            FluentValidation.Results.ValidationResult result = validator.Validate(command);

            // assert
            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_WhenGameIdIsGreaterThan0_ShouldBeTrue()
        {
            // arrange
            int gameId = 10;
            var command = new CheckAndUpdateGameWinConditionsCommand(gameId);
            var validator = new CheckAndUpdateGameWinConditionsCommandValidator();

            // act
            FluentValidation.Results.ValidationResult result = validator.Validate(command);

            // assert
            result.IsValid.ShouldBe(true);
        }
    }
}
