using FluentValidation;

namespace TicTacToe.Application.NoughtPlayerGameTiles.Commands.CreateNoughtPlayerGameTile
{
    public class CreateNoughtPlayerGameTileCommandValidator : AbstractValidator<CreateNoughtPlayerGameTileCommand>
    {
        public CreateNoughtPlayerGameTileCommandValidator()
        {
            RuleFor(x => x.X)
                .InclusiveBetween((byte)0, (byte)2).WithMessage("X must be in the range between 0 and 2.");
            RuleFor(x => x.Y)
                .InclusiveBetween((byte)0, (byte)2).WithMessage("Y must be in the range between 0 and 2.");
        }
    }
}
