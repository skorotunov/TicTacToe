using System;
using System.Threading;
using System.Threading.Tasks;

namespace TicTacToe.WebUI.Common
{
    public class SemaphoreLocker
    {
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public async Task LockAsync(Func<Task> worker)
        {
            await semaphore.WaitAsync();
            try
            {
                await worker();
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
