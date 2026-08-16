using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Cena_de_filosofos
{
    internal class MainApplication
    {
        public const int N = 5;

        static void Main(string[] args)
        {
            Fork[] forks = new Fork[N];
            Philosopher[] philosophers = new Philosopher[N];
            Thread[] threads = new Thread[N];

            for (int i = 0; i < N; i++)
            {
                forks[i] = new Fork(i);
            }

            for (int i = 0; i < N; i++)
            {
                Fork left = forks[i];
                Fork right = forks[(i + 1) % N];

                philosophers[i] =
                    new Philosopher(i, left, right);
            }

            for (int i = 0; i < N; i++)
            {
                threads[i] =
                    new Thread(philosophers[i].Run);

                threads[i].Start();
            }

            for (int i = 0; i < N; i++)
            {
                threads[i].Join();
            }

            Console.WriteLine();
            Console.WriteLine("Dinner is finished.");
        }
    }
}