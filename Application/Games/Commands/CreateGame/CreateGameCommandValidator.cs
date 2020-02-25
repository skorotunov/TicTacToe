using FluentValidation;

namespace TicTacToe.Application.Games.Commands.CreateGame
{
    public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
    {
        public CreateGameCommandValidator()
        {
            RuleFor(x => x.OpponentId)
                .MaximumLength(450)
                .NotEmpty().WithMessage("Player ID is required.");
        }
    }
}
