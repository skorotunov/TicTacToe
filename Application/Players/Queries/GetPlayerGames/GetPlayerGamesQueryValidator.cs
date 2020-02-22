using FluentValidation;

namespace TicTacToe.Application.Players.Queries.GetPlayerGames
{
    public class GetPlayerGamesQueryValidator : AbstractValidator<GetPlayerGamesQuery>
    {
        public GetPlayerGamesQueryValidator()
        {
            RuleFor(x => x.PlayerId)
                .NotEqual(x => x.CurrentPlayerId).WithMessage("Player ID should not be equal to the current player's ID.")
                .NotEmpty().WithMessage("Player ID is required.");
        }
    }
}
