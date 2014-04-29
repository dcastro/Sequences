using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class CombinationTests
    {
        [Fact]
        public void Combinations_CombinesElements()
        {
            var sequence = Sequence.Range(1, 6);
            var combs = sequence.Combinations(3).ToList();

            var expectedCombs = new[]
            {
                new[] {1, 2, 3},
                new[] {1, 2, 4},
                new[] {1, 2, 5},
                new[] {1, 3, 4},
                new[] {1, 3, 5},
                new[] {1, 4, 5},
                new[] {2, 3, 4},
                new[] {2, 3, 5},
                new[] {2, 4, 5},
                new[] {3, 4, 5}
            };

            Assert.Equal(expectedCombs.Length, combs.Count);

            for (int i = 0; i < combs.Count; i++)
                Assert.Equal(expectedCombs[i], combs[i]);
        }

        [Fact]
        public void Combinations_Excludes_Duplicates()
        {
            var combs = "abbbc".AsSequence().Combinations(2).ToList();

            var expectedCombs = new[]
            {
                new[] {'a', 'b'},
                new[] {'a', 'c'},
                new[] {'b', 'b'},
                new[] {'b', 'c'}
            };

            Assert.Equal(expectedCombs.Length, combs.Count);

            for (int i = 0; i < combs.Count; i++)
                Assert.Equal(expectedCombs[i], combs[i]);
        }

        [Fact]
        public void Combinations_Returns_NoSequences_When_SequenceIsEmpty()
        {
            var sequence = Sequence.Empty<int>();
            var combs = sequence.Combinations(2);

            Assert.Empty(combs);
        }

        [Fact]
        public void Combinations_Returns_OneEmptySequence_When_SizeIsZero()
        {
            var sequence = Sequence.Range(1, 6);
            var combs = sequence.Combinations(0).ToList();

            Assert.Equal(1, combs.Count);
            Assert.Empty(combs.First());
        }

        [Fact]
        public void Combinations_ThrowsException_When_SizeIsNegative()
        {
            var sequence = Sequence.Range(1, 6);
            Assert.Throws<ArgumentOutOfRangeException>(() => sequence.Combinations(-1));
        }
    }
}
