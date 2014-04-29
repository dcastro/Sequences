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

        [Fact]
        public void Remove_RemovesElement()
        {
            var sequence = Sequence.Range(1, 6);
            var result = sequence.Remove(3);
            var expectedResult = new[] {1, 2, 4, 5};

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void Remove_RemovesHead()
        {
            var sequence = Sequence.Range(1, 6);
            var result = sequence.Remove(1);
            var expectedResult = new[] {2, 3, 4, 5};

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void Remove_Returns_EquivalentSequence_When_ElementIsNotFound()
        {
            var sequence = Sequence.Range(1, 6);
            var result = sequence.Remove(10);
            var expectedResult = new[] {1, 2, 3, 4, 5};

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void Remove_Returns_SameSequence_When_SequenceIsEmpty()
        {
            var sequence = Sequence.Empty<int>();
            var result = sequence.Remove(10);

            Assert.Same(sequence, result);
        }
    }
}
