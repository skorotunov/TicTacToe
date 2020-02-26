using AutoMapper;

namespace TicTacToe.Application.Common.Mappings
{
    /// <summary>
    /// Automapper service to register mappings.
    /// </summary>
    /// <typeparam name="T">Type of the calss to register mapping.</typeparam>
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile)
        {
            profile.CreateMap(typeof(T), GetType());
        }
    }
}
