using System;
using TicTacToe.Application.Common.Interfaces;

namespace TicTacToe.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}
