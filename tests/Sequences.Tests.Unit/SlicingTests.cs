using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class SlicingTests
    {
        [Fact]
        public void Slice_ReturnsSubset()
        {
            var sequence = Sequence.From(0);
            var slice = sequence.Slice(4, 8);
            int[] expectedSlice = {4, 5, 6, 7};

            Assert.Equal(expectedSlice, slice);
        }

        [Fact]
        public void Slice_StartsAtZero_When_FromIsNegative()
        {
            var sequence = Sequence.From(0);
            var slice = sequence.Slice(-5, 5);
            int[] expectedSlice = {0, 1, 2, 3, 4};

            Assert.Equal(expectedSlice, slice);
        }

        [Fact]
        public void Slice_ReturnsEmptySequence_When_SequenceIsEmpty()
        {
            var sequence = Sequence.Empty<int>();
            var slice = sequence.Slice(0, 5);
            int[] expectedSlice = {};

            Assert.Equal(expectedSlice, slice);
        }

        [Fact]
        public void Slice_ReturnsEmptySequence_When_UntilIsLowerThanFrom()
        {
            var sequence = Sequence.From(0);
            var slice = sequence.Slice(5, 0);
            int[] expectedSlice = {};

            Assert.Equal(expectedSlice, slice);
        }

        [Fact]
        public void Slice_ReturnsShorterSlice_When_UntilIsHigherThanCount()
        {
            var sequence = Sequence.Range(0, 3, 1);
            var slice = sequence.Slice(1, 5);
            int[] expectedSlice = {1, 2};

            Assert.Equal(expectedSlice, slice);
        }
    }
}
