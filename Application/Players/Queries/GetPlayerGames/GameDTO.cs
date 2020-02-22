using AutoMapper;
using TicTacToe.Application.Common.Mappings;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Enums;

namespace TicTacToe.Application.Players.Queries.GetPlayerGames
{
    public class GameDTO : IMapFrom<Game>
    {
        public int Id { get; set; }

        public string StartDate { get; set; }

        public string Result { get; set; }

        public bool IsPossibleToContinue { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Game, GameDTO>()
                .ForMember(x => x.StartDate, opt => opt.MapFrom(x => x.StartDate.ToString("g")))
                .ForMember(x => x.IsPossibleToContinue, opt => opt.MapFrom(x => x.Result == GameResult.Active));
        }
    }
}
