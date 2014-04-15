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
            var longSequence = Sequence.From(1L);
            var bigIntSequence = Sequence.From((BigInteger) 1);

            Assert.Equal(expected, intSequence.Take(5));
            Assert.Equal(expected.Select(i => (long) i), longSequence.Take(5));
            Assert.Equal(expected.Select(i => (BigInteger) i), bigIntSequence.Take(5));
        }

        [Fact]
        public void FromInt_WithStep_Generates_IncreasingSequence()
        {
            int[] expected = {0, 5, 10, 15, 20};

            var intSequence = Sequence.From(0, 5);
            var longSequence = Sequence.From(0L, 5);
            var bigIntSequence = Sequence.From((BigInteger) 0, 5);

            Assert.Equal(expected, intSequence.Take(5));
            Assert.Equal(expected.Select(i => (long) i), longSequence.Take(5));
            Assert.Equal(expected.Select(i => (BigInteger) i), bigIntSequence.Take(5));
        }

        [Fact]
        public void Range_Generates_FiniteSequence()
        {
            int[] expected = {0, 5, 10, 15, 20};

            var intSequence = Sequence.Range(0, 25, 5);
            var longSequence = Sequence.Range(0L, 25L, 5L);
            var bigIntSequence = Sequence.Range((BigInteger) 0, 25, 5);

            Assert.Equal(expected, intSequence);
            Assert.Equal(expected.Select(i => (long) i), longSequence);
            Assert.Equal(expected.Select(i => (BigInteger) i), bigIntSequence);
        }

        [Fact]
        public void Range_Excludes_UpperBound()
        {
            Assert.DoesNotContain(25, Sequence.Range(0, 25, 5));
            Assert.DoesNotContain(25, Sequence.Range(0L, 25, 5));
            Assert.DoesNotContain(25, Sequence.Range((BigInteger) 0, 25, 5));
        }

        [Fact]
        public void Range_WithNegativeStep_Excludes_UpperBound()
        {
            Assert.DoesNotContain(0, Sequence.Range(25, 0, -5));
            Assert.DoesNotContain(0, Sequence.Range(25L, 0, -5));
            Assert.DoesNotContain(0, Sequence.Range((BigInteger) 25, 0, -5));
        }

        [Fact]
        public void Range_When_StartEqualsEnd_Generates_EmptySequence()
        {
            Assert.Empty(Sequence.Range(0, 0, 0));
            Assert.Empty(Sequence.Range(0L, 0, 0));
            Assert.Empty(Sequence.Range((BigInteger) 0, 0, 0));
        }
    }
}
