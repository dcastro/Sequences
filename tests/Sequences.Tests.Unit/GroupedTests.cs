using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class GroupedTests
    {
        [Fact]
        public void Grouped_GroupsElements()
        {
            var sequence = Sequence.Range(0, 6);
            var groups = sequence.Grouped(3).ToList();
            var expectedGroups = new[]
                {
                    new[] {0, 1, 2},
                    new[] {3, 4, 5}
                };

            Assert.Equal(expectedGroups.Length, groups.Count);

            for (int i = 0; i < groups.Count; i++)
                Assert.Equal(expectedGroups[i], groups[i]);
        }

        [Fact]
        public void Grouped_TruncatesLastGroup()
        {

            var sequence = Sequence.Range(0, 5);
            var groups = sequence.Grouped(3).ToList();
            var expectedGroups = new[]
                {
                    new[] {0, 1, 2},
                    new[] {3, 4}
                };

            Assert.Equal(expectedGroups.Length, groups.Count);

            for (int i = 0; i < groups.Count; i++)
                Assert.Equal(expectedGroups[i], groups[i]);
        }

        [Fact]
        public void Grouped_Returns_EmptyEnumerable_When_SequenceIsEmpty()
        {
            var sequence = Sequence.Empty<int>();
            var groups = sequence.Grouped(10);

            Assert.False(groups.Any());
        }

        [Fact]
        public void Grouped_ThrowsException_When_SizeIsNotPositive()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Sequence.From(0).Grouped(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => Sequence.From(0).Grouped(-1));
        }
    }
}
