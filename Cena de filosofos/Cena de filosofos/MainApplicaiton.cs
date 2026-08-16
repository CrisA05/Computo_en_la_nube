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

namespace Cena_de_filosofos
{
    internal class Philosopher
    {
        private int _id;
        private Fork _left;
        private Fork _right;
        private State _state;
        private int _eCount;

        public Philosopher(int id, Fork left, Fork right)
        {
            _id = id;
            _left = left;
            _right = right;
            _state = State.THINKING;
            _eCount = 0;
        }

        public void Run()
        {
            while (_eCount < MainApplication.N)
            {
                Think();

                _state = State.HUNGRY;
                Console.WriteLine(
                    $"Philosopher {_id} is HUNGRY."
                );

                TakeF();
                Eat();

                _left.Dispose();
                _right.Dispose();

                _state = State.THINKING;

                Console.WriteLine($"Philosopher {_id} released forks " + $"{_left.Id} and {_right.Id}.");

            }
        }

        private void Think()
        {
            _state = State.THINKING;

            Console.WriteLine(
                $"Philosopher {_id} is THINKING."
            );

            Thread.Sleep(Random.Shared.Next(1000, 2000));
        }

        private void Eat()
        {
            Console.WriteLine($"Philosopher {_id} is EATING.");

            Thread.Sleep(Random.Shared.Next(1000, 2000));

            _eCount++;
        }

        private void TakeF()
        {
            if (_id % 2 == 0)
            {
                _left.Take();
                _right.Take();
            }
            else
            {
                _right.Take();
                _left.Take();
            }

            _state = State.EATING;
            Console.WriteLine($"Philosopher {_id} took forks " + $"{_left.Id} and {_right.Id}.");
        }
    }
    internal enum State
    {
        THINKING,
        HUNGRY,
        EATING
    }
}

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