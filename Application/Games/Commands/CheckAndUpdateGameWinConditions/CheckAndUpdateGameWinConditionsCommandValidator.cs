using FluentValidation;

namespace TicTacToe.Application.Games.Commands.CheckAndUpdateGameWinConditions
{
    public class CheckAndUpdateGameWinConditionsCommandValidator : AbstractValidator<CheckAndUpdateGameWinConditionsCommand>
    {
        public CheckAndUpdateGameWinConditionsCommandValidator()
        {
            RuleFor(x => x.GameId)
                .GreaterThan(0).WithMessage("GmaeId must be possitive number");
        }
    }
}
