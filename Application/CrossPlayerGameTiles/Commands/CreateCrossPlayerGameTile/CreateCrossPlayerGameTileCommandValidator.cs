using FluentValidation;

namespace TicTacToe.Application.CrossPlayerGameTiles.Commands.CreateCrossPlayerGameTile
{
    /// <summary>
    /// Validator of the command to insert X's move data.
    /// </summary>
    public class CreateCrossPlayerGameTileCommandValidator : AbstractValidator<CreateCrossPlayerGameTileCommand>
    {
        public CreateCrossPlayerGameTileCommandValidator()
        {
            RuleFor(x => x.GameId)
                .GreaterThan(0).WithMessage("GameId must be possitive number");
            RuleFor(x => x.X)
                .InclusiveBetween((byte)0, (byte)2).WithMessage("X must be in the range between 0 and 2.");
            RuleFor(x => x.Y)
                .InclusiveBetween((byte)0, (byte)2).WithMessage("Y must be in the range between 0 and 2.");
        }
    }
}
