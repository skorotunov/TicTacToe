using System;
using TicTacToe.Application.Common.Interfaces;

namespace TicTacToe.Infrastructure.Services
{
    public class GuidService : IGuid
    {
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }
    }
}
