using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests.Functional
{
    public class PrimeNumbers
    {
        private readonly ISequence<int> _expectedPrimes = Sequence.With(2, 3, 5, 7, 11, 13, 17, 19, 23, 29,
                                                                        31, 37, 41, 43, 47, 53, 59, 61, 67, 71);

        [Fact]
        public void SieveOfEratosthenes()
        {
            /**
             * Generate integers in the range [2, Infinite) and then, for each index i:
             * 1) take the prime number p at position i of the input sequence
             * 2) remove all multiples of p (except p itself) from the input sequence 
             */
            var primes = Primes(Sequence.From(2), 0);

            Assert.Equal(_expectedPrimes, primes.Take(20));
        }

        [Fact]
        public void SieveOfEratosthenes_Optimized()
        {
            var primes = PrimesOptimized(Sequence.From(2));

            Assert.Equal(_expectedPrimes, primes.Take(20));
        }

        [Fact]
        public void SieveOfEratosthenes_Optimized_WithRange()
        {
            /**
             * The first two versions work well for the first ~1000 prime numbers, but then we start getting StackOverflow exceptions
             * That's because we apply a lazily-evaluated "Where" filter at each step.
             * So when you go evaluate the 1000th prime number, you have to apply 999 filters.
             * 
             * This version constrains the search for prime numbers to a range - here, [2,72) - and forces the evaluation of each filter
             * before movin onto the next step.
             */

            const int max = 72;
            var primes = PrimesWithin(Sequence.Range(2, max));

            Assert.Equal(_expectedPrimes, primes);
        }

        private ISequence<int> Primes(ISequence<int> seq, int index)
        {
            //take nth prime number
            var n = seq[index];

            //remove multiples of n, except n itself
            var filtered = seq.Where(e => e % n != 0 || e == n);

            return new Sequence<int>(n, () => Primes(filtered, index + 1));
        }

        private ISequence<int> PrimesOptimized(ISequence<int> seq)
        {
            //take the next prime number
            var n = seq.Head;

            //skip n, and remove further multiples of n
            var filtered = seq.Tail.Where(e => e % n != 0);

            return new Sequence<int>(n, () => PrimesOptimized(filtered));
        }

        private ISequence<int> PrimesWithin(ISequence<int> range)
        {
            if (range.IsEmpty)
                return range;

            //take the next prime number
            var p = range.Head;

            //skip p, and remove further multiples of p
            //force the evaluation of the filtered sequence, to avoid stacking filters
            var filtered = range.Tail.Where(e => e % p != 0).Force();

            return new Sequence<int>(p, () => PrimesWithin(filtered));
        }
    }
}
