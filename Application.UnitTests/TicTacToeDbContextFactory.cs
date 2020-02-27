using Microsoft.EntityFrameworkCore;
using System;
using TicTacToe.Domain.Entities;
using TicTacToe.Infrastructure.Persistence;

namespace TicTacToe.Application.UnitTests
{
    public static class TicTacToeDbContextFactory
    {
        public static TicTacToeDbContext Create()
        {
            DbContextOptions<TicTacToeDbContext> options = new DbContextOptionsBuilder<TicTacToeDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new TicTacToeDbContext(options);
            context.Database.EnsureCreated();
            SeedSampleData(context);
            return context;
        }

        /// <summary>
        /// Seed sample data for tests. In real world app it would be better to use sepate data per each tests class. 
        /// This approach was selected in the scope of this project due to the time concerns.
        /// </summary>
        /// <param name="context"></param>
        public static void SeedSampleData(TicTacToeDbContext context)
        {
            context.Games.AddRange(
                new Game { Id = 1, StartDate = new DateTime(2020, 2, 4), CrossPlayerId = "player1", NoughtPlayerId = "player2", Result = Domain.Enums.GameResult.Active, TurnNumber = 0 },
                new Game { Id = 2, StartDate = new DateTime(2020, 2, 14), CrossPlayerId = "player2", NoughtPlayerId = "player1", Result = Domain.Enums.GameResult.Draw, TurnNumber = 0 },
                new Game { Id = 3, StartDate = new DateTime(2020, 2, 19), CrossPlayerId = "player1", NoughtPlayerId = "player2", Result = Domain.Enums.GameResult.Win, TurnNumber = 0 },
                new Game { Id = 4, StartDate = new DateTime(2020, 2, 20), CrossPlayerId = "player2", NoughtPlayerId = "player1", Result = Domain.Enums.GameResult.Loss, TurnNumber = 0 },
                new Game { Id = 5, StartDate = new DateTime(2020, 2, 12), CrossPlayerId = "player1", NoughtPlayerId = "player3", Result = Domain.Enums.GameResult.Win, TurnNumber = 3 },
                new Game { Id = 6, StartDate = new DateTime(2020, 2, 6), CrossPlayerId = "player2", NoughtPlayerId = "player3", Result = Domain.Enums.GameResult.Active, TurnNumber = 0 },
                new Game { Id = 7, StartDate = new DateTime(2020, 2, 5), CrossPlayerId = "currentUserId", NoughtPlayerId = "player1", Result = Domain.Enums.GameResult.Active, TurnNumber = 5 },
                new Game { Id = 8, StartDate = new DateTime(2020, 2, 1), CrossPlayerId = "player3", NoughtPlayerId = "player2", Result = Domain.Enums.GameResult.Active, TurnNumber = 6 },
                new Game { Id = 9, StartDate = new DateTime(2020, 2, 3), CrossPlayerId = "player3", NoughtPlayerId = "player4", Result = Domain.Enums.GameResult.Active, TurnNumber = 4 },
                new Game { Id = 10, StartDate = new DateTime(2020, 2, 3), CrossPlayerId = "player3", NoughtPlayerId = "player4", Result = Domain.Enums.GameResult.Active, TurnNumber = 7 });

            context.Tiles.AddRange(
                new Tile { Id = 10, X = 3, Y = 3 });

            context.CrossPlayerGameTiles.AddRange(
                new CrossPlayerGameTile { GameId = 1, TileId = 10 },
                new CrossPlayerGameTile { GameId = 2, TileId = 10 },
                new CrossPlayerGameTile { GameId = 1, TileId = 10 },
                new CrossPlayerGameTile { GameId = 3, TileId = 10 },
                new CrossPlayerGameTile { GameId = 9, TileId = 1 },
                new CrossPlayerGameTile { GameId = 9, TileId = 2 },
                new CrossPlayerGameTile { GameId = 9, TileId = 3 },
                new CrossPlayerGameTile { GameId = 8, TileId = 2 },
                new CrossPlayerGameTile { GameId = 8, TileId = 6 },
                new CrossPlayerGameTile { GameId = 8, TileId = 4 },
                new CrossPlayerGameTile { GameId = 8, TileId = 7 },
                new CrossPlayerGameTile { GameId = 10, TileId = 3 },
                new CrossPlayerGameTile { GameId = 10, TileId = 5 },
                new CrossPlayerGameTile { GameId = 10, TileId = 9 },
                new CrossPlayerGameTile { GameId = 10, TileId = 4 });

            context.NoughtPlayerGameTiles.AddRange(
                new NoughtPlayerGameTile { GameId = 1, TileId = 10 },
                new NoughtPlayerGameTile { GameId = 2, TileId = 10 },
                new NoughtPlayerGameTile { GameId = 3, TileId = 10 },
                new NoughtPlayerGameTile { GameId = 4, TileId = 10 },
                new NoughtPlayerGameTile { GameId = 9, TileId = 5 },
                new NoughtPlayerGameTile { GameId = 9, TileId = 8 },
                new NoughtPlayerGameTile { GameId = 8, TileId = 1 },
                new NoughtPlayerGameTile { GameId = 8, TileId = 5 },
                new NoughtPlayerGameTile { GameId = 8, TileId = 9 },
                new NoughtPlayerGameTile { GameId = 10, TileId = 8 },
                new NoughtPlayerGameTile { GameId = 10, TileId = 7 },
                new NoughtPlayerGameTile { GameId = 10, TileId = 6 },
                new NoughtPlayerGameTile { GameId = 10, TileId = 1 });

            context.SaveChanges();
        }

        public static void Destroy(TicTacToeDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
