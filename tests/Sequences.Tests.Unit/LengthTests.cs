using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace Sequences.Tests
{
    public class LengthTests
    {
        [Fact]
        public void Count_Returns_SequenceLength()
        {
            var sequence = Sequence.With(1, 2, 3);
            Assert.Equal(3, sequence.Count);
        }

        [Fact]
        public void HasDefiniteSize_ReturnsFalse_When_SequenceHasNotBeenFullyEvaluated()
        {
            var sequence = Sequence.Range(0, 6, 1);

            sequence.Take(5).Force();
            
            Assert.False(sequence.HasDefiniteSize);
        }

        [Fact]
        public void HasDefiniteSize_ReturnsTrue_When_SequenceHasBeenEvaluated()
        {
            var sequence = Sequence.Range(0, 6, 1);

            sequence.Force();

            Assert.True(sequence.HasDefiniteSize);
        }

        [Fact]
        public void HasDefiniteSize_DoesntEvaluateTail()
        {
            var sequence = Sequence.Range(0, 6, 1);
            bool hasDefiniteSize = sequence.HasDefiniteSize;

            Assert.False(sequence.IsTailDefined);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 0)]
        [InlineData(4, -1)]
        public void LengthCompare_ReturnsExpectedResult_When_CountEqualsThree(int length, int expectedResult)
        {
            var sequence = Sequence.Range(1, 4);
            Assert.Equal(expectedResult, sequence.LengthCompare(length));
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        public void LengthCompare_ReturnsExpectedResult_When_SequenceIsInfinite(int length, int expectedResult)
        {
            var sequence = Sequence.From(1);
            Assert.Equal(expectedResult, sequence.LengthCompare(length));
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(0, 0)]
        [InlineData(1, -1)]
        public void LengthCompare_ReturnsExpectedResult_When_SequenceIsEmpty(int length, int expectedResult)
        {
            var sequence = Sequence.Empty<int>();
            Assert.Equal(expectedResult, sequence.LengthCompare(length));
        }
    }
}
