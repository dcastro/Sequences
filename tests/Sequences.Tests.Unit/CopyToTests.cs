using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class CopyToTests
    {
        [Fact]
        public void CopyTo_CopiesWholeSequence()
        {
            var sequence = Sequence.Range(1, 6);
            var copy = new int[5];
            int[] expectedCopy = {1, 2, 3, 4, 5};

            int copied = sequence.CopyTo(copy);

            Assert.Equal(5, copied);
            Assert.Equal(expectedCopy, copy);
        }

        [Fact]
        public void CopyTo_StopsWhenAllElementsHaveBeenCopied()
        {
            var sequence = Sequence.Range(1, 4);
            var copy = new int[5];
            int[] expectedCopy = {1, 2, 3, 0, 0};

            int copied = sequence.CopyTo(copy);

            Assert.Equal(3, copied);
            Assert.Equal(expectedCopy, copy);
        }

        [Fact]
        public void CopyTo_StopsWhenArrayIsFull()
        {
            var sequence = Sequence.Range(1, 10);
            var copy = new int[5];
            int[] expectedCopy = {1, 2, 3, 4, 5};

            int copied = sequence.CopyTo(copy);

            Assert.Equal(5, copied);
            Assert.Equal(expectedCopy, copy);
        }

        [Fact]
        public void CopyTo_StopsWhenNElementsHaveBeenCopied()
        {
            var sequence = Sequence.Range(1, 10);
            var copy = new int[5];
            int[] expectedCopy = {1, 2, 3, 0, 0};

            int copied = sequence.CopyTo(0, copy, 0, 3);

            Assert.Equal(3, copied);
            Assert.Equal(expectedCopy, copy);
        }

        [Fact]
        public void CopyTo_SkipsOriginOffset()
        {
            var sequence = Sequence.Range(1, 10);
            var copy = new int[5];
            int[] expectedCopy = {3, 4, 5, 0, 0};

            int copied = sequence.CopyTo(2, copy, 0, 3);

            Assert.Equal(3, copied);
            Assert.Equal(expectedCopy, copy);
        }

        [Fact]
        public void CopyTo_SkipsDestinationOffset()
        {
            var sequence = Sequence.Range(1, 10);
            var copy = new int[5];
            int[] expectedCopy = {0, 0, 1, 2, 3};

            int copied = sequence.CopyTo(0, copy, 2, 10);

            Assert.Equal(3, copied);
            Assert.Equal(expectedCopy, copy);
        }

        [Fact]
        public void CopyTo_ThrowsException_When_DestinationOffsetIsNegative()
        {
            var sequence = Sequence.Range(1, 6);
            var copy = new int[5];

            Assert.Throws<ArgumentOutOfRangeException>(() => sequence.CopyTo(copy, -1));
        }

        [Fact]
        public void CopyTo_ThrowsException_When_OriginOffsetIsNegative()
        {
            var sequence = Sequence.Range(1, 6);
            var copy = new int[5];

            Assert.Throws<ArgumentOutOfRangeException>(() => sequence.CopyTo(-1, copy, 0, 1));
        }

        [Fact]
        public void CopyTo_ThrowsException_When_CountIsNegative()
        {
            var sequence = Sequence.Range(1, 6);
            var copy = new int[5];

            Assert.Throws<ArgumentOutOfRangeException>(() => sequence.CopyTo(0, copy, 0, -1));
        }
    }
}
