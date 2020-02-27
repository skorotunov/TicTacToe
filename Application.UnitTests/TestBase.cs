using AutoMapper;
using Moq;
using System;
using TicTacToe.Application.Common.Interfaces;
using TicTacToe.Application.Common.Mappings;
using TicTacToe.Infrastructure.Persistence;

namespace TicTacToe.Application.UnitTests
{
    public class TestBase : IDisposable
    {
        public TestBase()
        {
            Context = TicTacToeDbContextFactory.Create();
            var configurationProvider = new MapperConfiguration(x =>
            {
                x.AddProfile<MappingProfile>();
            });

            Mapper = configurationProvider.CreateMapper();

            var dateTimeMock = new Mock<IDateTime>();
            dateTimeMock.Setup(x => x.Now)
                .Returns(new DateTime(2020, 1, 1));
            DateTime = dateTimeMock.Object;

            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(x => x.UserId)
                .Returns("currentUserId");
            CurrentUserService = currentUserServiceMock.Object;
        }

        public TicTacToeDbContext Context { get; }

        public IMapper Mapper { get; }

        public IDateTime DateTime { get; }

        public ICurrentUserService CurrentUserService { get; }

        public void Dispose()
        {
            TicTacToeDbContextFactory.Destroy(Context);
        }
    }
}
