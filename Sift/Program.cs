using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sift
{
    class Program
    {
        static void Main(string[] args)
        {
            Sieve sieve = new Sieve(50_000_000);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            sieve.ComputeSieve();
            stopwatch.Stop();
            Console.WriteLine($"Computed {sieve.Primes.Count} primes in {stopwatch.ElapsedMilliseconds} ms.");

            stopwatch.Start();
            sieve.VerifyComputation(8);
            stopwatch.Stop();
            Console.WriteLine($"Verified the sieve in {stopwatch.ElapsedMilliseconds} ms.");


            Console.WriteLine("Done. Press any key to continue...");
            Console.ReadKey();
        }
    }
}
