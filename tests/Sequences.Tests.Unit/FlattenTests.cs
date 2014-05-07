using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class FlattenTests
    {
        [Fact]
        public void Flatten_SequenceOfEnumerables_ConcatenatesEnumerables()
        {
            ISequence<IEnumerable<int>> sequence =
                Sequence.With<IEnumerable<int>>(
                    new[] {1, 2, 3},
                    new int[] {},
                    new[] {4, 5},
                    new[] {6, 7, 8},
                    new int[] {}
                    );

            var flattened = sequence.Flatten();
            int[] expectedFlattened = {1, 2, 3, 4, 5, 6, 7, 8};

            Assert.Equal(expectedFlattened, flattened);
        }

        [Fact]
        public void Flatten_EnumerableOfSequences_ConcatenatesSequences()
        {
            IEnumerable<ISequence<int>> sequence =
                new[]
                    {
                        Sequence.With(1, 2, 3),
                        Sequence.Empty<int>(),
                        Sequence.With(4, 5),
                        Sequence.With(6, 7, 8),
                        Sequence.Empty<int>()
                    };

            var flattened = sequence.Flatten();
            int[] expectedFlattened = {1, 2, 3, 4, 5, 6, 7, 8};

            Assert.Equal(expectedFlattened, flattened);
        }

        [Fact]
        public void Flatten_SequenceOfSequences_ConcatenatesSequences()
        {
            ISequence<ISequence<int>> sequence =
                Sequence.With(
                    Sequence.With(1, 2, 3),
                    Sequence.Empty<int>(),
                    Sequence.With(4, 5),
                    Sequence.With(6, 7, 8),
                    Sequence.Empty<int>()
                    );

            var flattened = sequence.Flatten();
            int[] expectedFlattened = {1, 2, 3, 4, 5, 6, 7, 8};

            Assert.Equal(expectedFlattened, flattened);
        }
    }
}
