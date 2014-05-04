using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class PermutationsTests
    {
        [Fact]
        public void Permutations_RearrangesElements()
        {
            var sequence = Sequence.Range(1, 4);
            var permutations = sequence.Permutations().ToList();

            var expectedPermutations = new[]
            {
                new[] {1, 2, 3},
                new[] {1, 3, 2},
                new[] {2, 1, 3},
                new[] {2, 3, 1},
                new[] {3, 1, 2},
                new[] {3, 2, 1}
            };

            Assert.Equal(expectedPermutations.Length, permutations.Count);

            for (int i = 0; i < permutations.Count; i++)
                Assert.Equal(expectedPermutations[i], permutations[i]);
        }

        [Fact]
        public void Permutations_Excludes_Duplicates()
        {
            var permutations = "abb".AsSequence().Permutations().ToList();

            var expectedPermutations = new[]
            {
                new[] {'a', 'b', 'b'},
                new[] {'b', 'a', 'b'},
                new[] {'b', 'b', 'a'}
            };

            Assert.Equal(expectedPermutations.Length, permutations.Count);

            for (int i = 0; i < permutations.Count; i++)
                Assert.Equal(expectedPermutations[i], permutations[i]);
        }

        [Fact]
        public void Permutations_Returns_OneEmptySequence_When_SequenceIsEmpty()
        {
            var sequence = Sequence.Empty<int>();
            var permutations = sequence.Permutations().ToList();

            Assert.Equal(1, permutations.Count);
            Assert.Empty(permutations.First());
        }
    }
}
