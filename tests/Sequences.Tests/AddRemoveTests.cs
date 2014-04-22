using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class AddRemoveTests
    {
        [Fact]
        public void Concat_ConcatenatesTwoSequences()
        {
            var first = Sequence.For(1, 2);
            var second = Sequence.For(3, 4);

            Assert.Equal(new[] {1, 2, 3, 4},
                         first.Concat(() => second));
        }

        [Fact]
        public void Concat_ConcatenatesEmpty_WithNonEmpty()
        {
            var first = Sequence.Empty<int>();
            var second = Sequence.For(1, 2, 3, 4);

            Assert.Equal(new[] {1, 2, 3, 4},
                         first.Concat(() => second));
        }

        [Fact]
        public void Concat_ConcatenatesNonEmpty_WithEmpty()
        {
            var first = Sequence.For(1, 2, 3, 4);
            var second = Sequence.Empty<int>();

            Assert.Equal(new[] {1, 2, 3, 4},
                         first.Concat(() => second));
        }

        [Fact]
        public void Append_AppendsElementToSequence()
        {
            Assert.Equal(new[] {1, 2, 3, 4},
                         Sequence.For(1, 2, 3).Append(4));
        }

        [Fact]
        public void Prepend_PrependsElementToSequence()
        {
            Assert.Equal(new[] {1, 2, 3, 4},
                         Sequence.For(2, 3, 4).Prepend(1));
        }
    }
}
