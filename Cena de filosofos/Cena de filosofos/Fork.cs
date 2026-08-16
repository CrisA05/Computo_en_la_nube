using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Cena_de_filosofos
{
    internal class Fork
    {
        public int Id { get; }
        private readonly SemaphoreSlim _semaphore;


        public Fork(int id) { Id = id; _semaphore = new SemaphoreSlim(1, 1); }

        public void Take()
        {
            _semaphore.Wait();
        }
        public void Dispose()
        {
            _semaphore.Release();
        }
    }
}
