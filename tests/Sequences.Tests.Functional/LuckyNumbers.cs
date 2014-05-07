using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests.Functional
{
    public class LuckyNumbers
    {
        private readonly ISequence<int> _expectedLuckyNumbers = Sequence.With(
            1, 3, 7, 9, 13, 15, 21, 25, 31, 33, 37, 43, 49, 51, 63, 67, 69, 73, 75, 79);

        [Fact]
        public void V1()
        {
            var luckies = Lucky();

            Assert.Equal(_expectedLuckyNumbers, luckies.Take(20));
        }

        private ISequence<int> Lucky()
        {
            var oddInts = Sequence.Iterate(1, e => e + 2);
            return new Sequence<int>(1, () => Lucky(oddInts, 1));
        }

        private ISequence<int> Lucky(ISequence<int> seq, int index)
        {
            //select the next element
            var n = seq[index];

            //remove every nth element from the sequence
            var filtered = seq.Sliding(n - 1, n).Flatten();

            return new Sequence<int>(n, () => Lucky(filtered, index + 1));
        }
    }
}
