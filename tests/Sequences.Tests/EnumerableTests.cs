using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class EnumerableTests
    {
        [Fact]
        public void Enumerator_IteratesThroughElements()
        {
            var sequence = new Sequence<int>(1, () =>
                                new Sequence<int>(2, () =>
                                    new Sequence<int>(3, () =>
                                        Sequence<int>.Empty)));

            Assert.Equal(new []{1,2,3}, sequence);
        }
    }
}
