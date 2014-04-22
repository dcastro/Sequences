using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class LengthTests
    {
        [Fact]
        public void Count_Returns_SequenceLength()
        {
            var sequence = Sequence.For(1, 2, 3);
            Assert.Equal(3, sequence.Count);
        }

        [Fact]
        public void HasDefiniteSize_ReturnsFalse_When_SequenceHasNotBeenFullyEvaluated()
        {
            var sequence = Sequence.Range(0, 6, 1);

            foreach (var elem in sequence.Take(5))
            {
            }

            Assert.False(sequence.HasDefiniteSize);
        }

        [Fact]
        public void HasDefiniteSize_ReturnsTrue_When_SequenceHasBeenEvaluated()
        {
            var sequence = Sequence.Range(0, 6, 1);

            foreach (var elem in sequence)
            {
            }

            Assert.True(sequence.HasDefiniteSize);
        }

        [Fact]
        public void HasDefiniteSize_DoesntEvaluateTail()
        {
            var sequence = Sequence.Range(0, 6, 1);
            bool hasDefiniteSize = sequence.HasDefiniteSize;

            Assert.False(sequence.IsTailDefined);
        }

    }
}
