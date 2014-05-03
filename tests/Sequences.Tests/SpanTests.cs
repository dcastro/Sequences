using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class SpanTests
    {
        [Fact]
        public void Span_SplitsIntoPrefixAndSuffix()
        {
            var sequence = Sequence.Range(1, 11);
            var parts = sequence.Span(e => e <= 5);

            int[] expectedFirst = {1, 2, 3, 4, 5};
            int[] expectedSecond = {6, 7, 8, 9, 10};

            Assert.Equal(expectedFirst, parts.Item1);
            Assert.Equal(expectedSecond, parts.Item2);
        }
    }
}
