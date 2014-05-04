using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class PadToTests
    {
        [Fact]
        public void PadTo_AppendsElements()
        {
            var sequence = Sequence.Range(1, 4);
            var padded = sequence.PadTo(5, 0);
            int[] expectedPadded = {1, 2, 3, 0, 0};

            Assert.Equal(expectedPadded, padded);
        }

        [Fact]
        public void PadTo_DoesNothing_When_LengthIsLessThanCount()
        {
            var sequence = Sequence.Range(1, 4);
            var padded = sequence.PadTo(2, 0);
            int[] expectedPadded = {1, 2, 3};

            Assert.Equal(expectedPadded, padded);
        }

        [Fact]
        public void PadTo_DoesNothing_When_LengthIsEqualToCount()
        {
            var sequence = Sequence.Range(1, 4);
            var padded = sequence.PadTo(3, 0);
            int[] expectedPadded = {1, 2, 3};

            Assert.Equal(expectedPadded, padded);
        }
    }
}
