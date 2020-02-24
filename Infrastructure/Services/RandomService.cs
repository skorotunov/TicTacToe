using System;
using TicTacToe.Application.Common.Interfaces;

namespace TicTacToe.Infrastructure.Services
{
    public class RandomService : IRandom
    {
        private readonly IGuid guid;

        public RandomService(IGuid guid)
        {
            this.guid = guid;
        }

        public int Next(int minValue, int maxValue)
        {
            return new Random(guid.NewGuid().GetHashCode()).Next(minValue, maxValue);
        }
    }
}
