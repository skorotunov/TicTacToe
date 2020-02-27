using FluentValidation;

namespace TicTacToe.Application.Players.Queries.GetPlayerGames
{
    /// <summary>
    /// Validator for the get player games query parameters.
    /// </summary>
    public class GetPlayerGamesQueryValidator : AbstractValidator<GetPlayerGamesQuery>
    {
        public GetPlayerGamesQueryValidator()
        {
            RuleFor(x => x.PlayerId)
                .NotEqual(x => x.CurrentPlayerId).WithMessage("Player ID should not be equal to the current player's ID.")
                .NotEmpty().WithMessage("Player ID is required.");
            RuleFor(x => x.CurrentPlayerId)
                .NotEmpty().WithMessage("Current Player ID is required.");
        }
    }
}
