using AutoMapper;
using TicTacToe.Application.Common.Mappings;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Enums;

namespace TicTacToe.Application.Players.Queries.GetPlayerGames
{
    /// <summary>
    /// Game DTO object whic represents info about Game entity.
    /// </summary>
    public class GameDTO : IMapFrom<Game>
    {
        public int Id { get; set; }

        public string StartDate { get; set; }

        public string Result { get; set; }

        public void Mapping(Profile profile)
        {
            // map result property differently based on the current user
            string currentUserId = null;
            profile.CreateMap<Game, GameDTO>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString("g")))
                .ForMember(
                    dest => dest.Result,
                    opt => opt.MapFrom(src => src.Result == GameResult.Win && src.NoughtPlayerId == currentUserId
                        ? GameResult.Loss.ToString()
                        : (src.Result == GameResult.Loss && src.NoughtPlayerId == currentUserId ? GameResult.Win.ToString() : src.Result.ToString())));
        }
    }
}
