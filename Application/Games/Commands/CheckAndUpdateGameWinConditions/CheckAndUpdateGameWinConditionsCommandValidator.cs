using FluentValidation;

namespace TicTacToe.Application.Games.Commands.CheckAndUpdateGameWinConditions
{
    /// <summary>
    /// Command validation rules.
    /// </summary>
    public class CheckAndUpdateGameWinConditionsCommandValidator : AbstractValidator<CheckAndUpdateGameWinConditionsCommand>
    {
        public CheckAndUpdateGameWinConditionsCommandValidator()
        {
            RuleFor(x => x.GameId)
                .GreaterThan(0).WithMessage("GameId must be possitive number");
        }
    }
}
