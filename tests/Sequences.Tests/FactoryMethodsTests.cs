using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class FactoryMethodsTests
    {
        [Fact]
        public void FromInt_Generates_ConsecutiveIntegers()
        {
            int[] expected = {1, 2, 3, 4, 5};

            var intSequence = Sequence.From(1);
            Assert.Equal(expected, intSequence.Take(5));

            var longSequence = Sequence.From(1L);
            Assert.Equal(expected.Select(i => (long) i), longSequence.Take(5));

            var bigIntSequence = Sequence.From((BigInteger) 1);
            Assert.Equal(expected.Select(i => (BigInteger) i), bigIntSequence.Take(5));
        }

        [Fact]
        public void FromInt_WithStep_Generates_IncreasingSequence()
        {
            int[] expected = {0, 5, 10, 15, 20};

            var intSequence = Sequence.From(0, 5);
            Assert.Equal(expected, intSequence.Take(5));

            var longSequence = Sequence.From(0L, 5);
            Assert.Equal(expected.Select(i => (long) i), longSequence.Take(5));

            var bigIntSequence = Sequence.From((BigInteger) 0, 5);
            Assert.Equal(expected.Select(i => (BigInteger) i), bigIntSequence.Take(5));
        }
    }
}
