using AutoMapper;
using System;
using TicTacToe.Application.Games.Queries.GetGameTiles;
using TicTacToe.Application.Players.Queries.GetPlayerGames;
using TicTacToe.Domain.Entities;
using Xunit;

namespace TicTacToe.Application.UnitTests.Common.Mappings
{
    public class MappingTests : IClassFixture<MappingTestsFixture>
    {
        private readonly IMapper mapper;
        private readonly IConfigurationProvider configuration;

        public MappingTests(MappingTestsFixture fixture)
        {
            mapper = fixture.Mapper;
            configuration = fixture.ConfigurationProvider;
        }

        [Fact]
        public void Mapping_Always_ShouldHaveValidConfiguration()
        {
            configuration.AssertConfigurationIsValid();
        }

        [Theory]
        [InlineData(typeof(Game), typeof(GameDTO))]
        [InlineData(typeof(Tile), typeof(TileDTO))]
        [InlineData(typeof(CrossPlayerGameTile), typeof(CrossPlayerTileDTO))]
        [InlineData(typeof(NoughtPlayerGameTile), typeof(NoughtPlayerTileDTO))]
        public void Mapping_Always_ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            object instance = Activator.CreateInstance(source);
            mapper.Map(instance, source, destination);
        }
    }
}
