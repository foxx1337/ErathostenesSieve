using System;
using System.Collections.Generic;
using System.Threading;

namespace Sift
{
    public class Sieve
    {
        public long Size { get; }

        public IList<long> Primes { get; }

        public Sieve(long size)
        {
            Size = size;
            Primes = new List<long>();

            _numberSieve = new bool[Size];
            _numberSieve[0] = _numberSieve[1] = false;
            for (int i = 2; i < Size; i++)
            {
                _numberSieve[i] = true;
            }

            _computed = false;
        }

        public void ComputeSieve()
        {
            for (long number = NextPrime(0); number < Size; number = NextPrime(number))
            {
                Primes.Add(number);
                for (long newNumber = number * number; newNumber < Size; newNumber += number)
                {
                    _numberSieve[newNumber] = false;
                }
            }

            _computed = true;
        }

        public void VerifyComputation(int threads = 1)
        {
            if (!_computed)
            {
                throw new Exception(
                    $"Must first call {nameof(ComputeSieve)} before calling {nameof(VerifyComputation)}.");
            }

            ThreadPool.SetMaxThreads(threads, 1);

            using (var countDownEvent = new CountdownEvent(threads))
            {
                for (long i = 0; i < threads; i++)
                {
                    var start = i * Size / threads;
                    var stop = (i + 1) * Size / threads;
                    ThreadPool.QueueUserWorkItem((state) =>
                        {
                            VerifySlice(start, stop);
                            countDownEvent.Signal();
                        }
                    );
                }

                countDownEvent.Wait();
            }
        }

        private void VerifySlice(long start, long stop)
        {
            Console.WriteLine($"Verifying slice between {start} and {stop}.");

            for (long i = start; i < stop; i++)
            {
                long sqrt = (long) Math.Sqrt(i);
                if (_numberSieve[i])
                {
                    for (int factor = 2; factor <= sqrt; factor++)
                    {
                        if (i % factor == 0)
                        {
                            throw new Exception($"Number {i} was thought prime but it's divisible by {factor}.");
                        }
                    }
                }
            }
        }

        private long NextPrime(long currentNumber)
        {
            long candidate = currentNumber + 1;
            while (candidate < Size && !_numberSieve[candidate])
            {
                candidate++;
            }

            return candidate;
        }

        private readonly bool[] _numberSieve;

        private bool _computed;
    }
}