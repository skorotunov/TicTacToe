using Shouldly;
using TicTacToe.Application.CrossPlayerGameTiles.Commands.CreateCrossPlayerGameTile;
using Xunit;

namespace TicTacToe.Application.UnitTests.CrossPlayerGameTiles.Commands.CreateCrossPlayerGameTile
{
    public class CreateCrossPlayerGameTileCommandValidatorTests : TestBase
    {
        [Fact]
        public void IsValid_WhenGameIdIsNought_ShouldBeFalse()
        {
            // arrange
            int gameId = 0;
            byte x = 2;
            byte y = 2;
            var command = new CreateCrossPlayerGameTileCommand()
            {
                GameId = gameId,
                X = x,
                Y = y
            };
            var validator = new CreateCrossPlayerGameTileCommandValidator();

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
            byte x = 2;
            byte y = 2;
            var command = new CreateCrossPlayerGameTileCommand()
            {
                GameId = gameId,
                X = x,
                Y = y
            };
            var validator = new CreateCrossPlayerGameTileCommandValidator();

            // act
            FluentValidation.Results.ValidationResult result = validator.Validate(command);

            // assert
            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_WhenXIsGreaterThan2_ShouldBeFalse()
        {
            // arrange
            int gameId = 1;
            byte x = 3;
            byte y = 2;
            var command = new CreateCrossPlayerGameTileCommand()
            {
                GameId = gameId,
                X = x,
                Y = y
            };
            var validator = new CreateCrossPlayerGameTileCommandValidator();

            // act
            FluentValidation.Results.ValidationResult result = validator.Validate(command);

            // assert
            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_WhenYIsGreaterThan2_ShouldBeFalse()
        {
            // arrange
            int gameId = 1;
            byte x = 2;
            byte y = 3;
            var command = new CreateCrossPlayerGameTileCommand()
            {
                GameId = gameId,
                X = x,
                Y = y
            };
            var validator = new CreateCrossPlayerGameTileCommandValidator();

            // act
            FluentValidation.Results.ValidationResult result = validator.Validate(command);

            // assert
            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_WhenGameIdIsGreaterThan0AndXYAreLessThanOrEqualTo2_ShouldBeTrue()
        {
            // arrange
            int gameId = 1;
            byte x = 2;
            byte y = 0;
            var command = new CreateCrossPlayerGameTileCommand()
            {
                GameId = gameId,
                X = x,
                Y = y
            };
            var validator = new CreateCrossPlayerGameTileCommandValidator();

            // act
            FluentValidation.Results.ValidationResult result = validator.Validate(command);

            // assert
            result.IsValid.ShouldBe(true);
        }
    }
}
