using FluentValidation;

namespace TicTacToe.Application.NoughtPlayerGameTiles.Commands.CreateNoughtPlayerGameTile
{
    public class CreateNoughtPlayerGameTileCommandValidator : AbstractValidator<CreateNoughtPlayerGameTileCommand>
    {
        /// <summary>
        /// Validator of the command to insert O's move data.
        /// </summary>
        public CreateNoughtPlayerGameTileCommandValidator()
        {
            RuleFor(x => x.GameId)
                .GreaterThan(0).WithMessage("GmaeId must be possitive number");
            RuleFor(x => x.X)
                .InclusiveBetween((byte)0, (byte)2).WithMessage("X must be in the range between 0 and 2.");
            RuleFor(x => x.Y)
                .InclusiveBetween((byte)0, (byte)2).WithMessage("Y must be in the range between 0 and 2.");
        }
    }
}
