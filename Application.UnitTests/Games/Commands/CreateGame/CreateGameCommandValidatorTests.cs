using Shouldly;
using TicTacToe.Application.Games.Commands.CreateGame;
using Xunit;

namespace TicTacToe.Application.UnitTests.Games.Commands.CreateGame
{
    public class CreateGameCommandValidatorTests : TestBase
    {
        [Fact]
        public void IsValid_WhenOpponentIdIsEmpty_ShouldBeFalse()
        {
            // arrange
            string opponentId = string.Empty;
            bool isOpponentCrossPlayer = false;
            var command = new CreateGameCommand()
            {
                OpponentId = opponentId,
                IsOpponentCrossPlayer = isOpponentCrossPlayer
            };
            var validator = new CreateGameCommandValidator();

            // act
            FluentValidation.Results.ValidationResult result = validator.Validate(command);

            // assert
            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_WhenOpponentIdIsNull_ShouldBeFalse()
        {
            // arrange
            string opponentId = null;
            bool isOpponentCrossPlayer = false;
            var command = new CreateGameCommand()
            {
                OpponentId = opponentId,
                IsOpponentCrossPlayer = isOpponentCrossPlayer
            };
            var validator = new CreateGameCommandValidator();

            // act
            FluentValidation.Results.ValidationResult result = validator.Validate(command);

            // assert
            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_WhenOpponentIdIsLongerThan450_ShouldBeFalse()
        {
            // arrange
            string opponentId = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque quis laoreet odio. Ut consequat lacinia ex, ut suscipit ligula ultrices sed. Duis quis libero eu ipsum convallis tempus. Sed malesuada augue pulvinar aliquam facilisis. Nulla fermentum enim quis convallis iaculis. Aliquam eu varius magna. Sed quis metus placerat, eleifend orci sed, tempus ligula. Duis ut egestas ante. Proin a purus ac erat gravida aliquam quis vitae justo. Mauris id.";
            bool isOpponentCrossPlayer = false;
            var command = new CreateGameCommand()
            {
                OpponentId = opponentId,
                IsOpponentCrossPlayer = isOpponentCrossPlayer
            };
            var validator = new CreateGameCommandValidator();

            // act
            FluentValidation.Results.ValidationResult result = validator.Validate(command);

            // assert
            result.IsValid.ShouldBe(false);
        }

        [Fact]
        public void IsValid_WhenOpponentIdIsNotEmptyOrNullAndIsShorterThan450_ShouldBeTrue()
        {
            // arrange
            string opponentId = "opponentId";
            bool isOpponentCrossPlayer = false;
            var command = new CreateGameCommand()
            {
                OpponentId = opponentId,
                IsOpponentCrossPlayer = isOpponentCrossPlayer
            };
            var validator = new CreateGameCommandValidator();

            // act
            FluentValidation.Results.ValidationResult result = validator.Validate(command);

            // assert
            result.IsValid.ShouldBe(true);
        }
    }
}
