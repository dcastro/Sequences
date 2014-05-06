using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests.Functional
{
    public class TriangularNumbers
    {
        private readonly ISequence<int> _expectedNumbers = Sequence.With(0, 1, 3, 6, 10, 15, 21, 28, 36, 45);

        [Fact]
        public void V1()
        {
            var numbers = Sequence.From(0)
                                  .Select(n => n*(n + 1)/2);

            Assert.Equal(_expectedNumbers, numbers.Take(10));
        }

        [Fact]
        public void V2()
        {
            var numbers = Triangular(0, 0);

            Assert.Equal(_expectedNumbers, numbers.Take(10));
        }

        private ISequence<int> Triangular(int triangularNumber, int index)
        {
            return new Sequence<int>(triangularNumber, () => Triangular(triangularNumber + index + 1, index + 1));
        } 
    }
}
