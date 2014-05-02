using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class SplitAtTests
    {
        [Fact]
        public void SplitAt_SplitsAtSelectedIndex()
        {
            var sequence = Sequence.Range(1, 6);
            var parts = sequence.SplitAt(3);

            int[] expectedFirst = {1, 2, 3};
            int[] expectedSecond = {4, 5};

            Assert.Equal(expectedFirst, parts.Item1);
            Assert.Equal(expectedSecond, parts.Item2);
        }
    }
}
