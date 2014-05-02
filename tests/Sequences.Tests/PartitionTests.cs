using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class PartitionTests
    {
        [Fact]
        public void Partition_SelectsElementsUsingAPredicate()
        {
            var sequence = Sequence.From(1);
            var parts = sequence.Partition(e => e%2 == 0);

            int[] expectedFirst = {2, 4, 6, 8, 10};
            int[] expectedSecond = {1, 3, 5, 7, 9};

            Assert.Equal(expectedFirst, parts.Item1.Take(5));
            Assert.Equal(expectedSecond, parts.Item2.Take(5));
        }
    }
}
