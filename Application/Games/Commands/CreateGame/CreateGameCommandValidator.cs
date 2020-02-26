using FluentValidation;

namespace TicTacToe.Application.Games.Commands.CreateGame
{
    /// <summary>
    /// Command validation rules.
    /// </summary>
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
