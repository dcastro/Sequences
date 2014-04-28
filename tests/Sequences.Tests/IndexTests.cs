using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class IndexTests
    {
        [Fact]
        public void Indices_Returns_RangeOfIndices()
        {
            var sequence = Sequence.Fill(0, 5);
            var indices = sequence.Indices;
            int[] expectedIndices = {0, 1, 2, 3, 4};

            Assert.Equal(expectedIndices, indices);
        }

        [Fact]
        public void IndexOf_Returns_IndexOfElement()
        {
            var sequence = Sequence.Range(0, 5);
            var index = sequence.IndexOf(3);

            Assert.Equal(3, index);
        }

        [Fact]
        public void IndexOf_Returns_MinusOne_When_ElementIsNotFound()
        {
            var sequence = Sequence.Range(0, 5);
            var index = sequence.IndexOf(5);

            Assert.Equal(-1, index);
        }

        [Fact]
        public void IndexOf_Returns_MinusOne_When_ElementIsBeforeFrom()
        {
            var sequence = Sequence.Range(0, 5);
            var index = sequence.IndexOf(2, from: 3);

            Assert.Equal(-1, index);
        }

        [Fact]
        public void IndexOf_WithCount_Returns_MinusOne_When_ElementIsNotWithinRange()
        {
            var sequence = Sequence.Range(0, 5);
            var index = sequence.IndexOf(3, from: 0, count: 3);

            Assert.Equal(-1, index);
        }

        [Fact]
        public void IndexWhere_Returns_IndexOfElement()
        {
            var sequence = Sequence.Range(0, 5);
            var index = sequence.IndexWhere(i => i == 3);

            Assert.Equal(3, index);
        }

        [Fact]
        public void IndexWhere_Returns_MinusOne_When_ElementIsNotFound()
        {
            var sequence = Sequence.Range(0, 5);
            var index = sequence.IndexWhere(i => i == 5);

            Assert.Equal(-1, index);
        }

        [Fact]
        public void IndexWhere_Returns_MinusOne_When_ElementIsBeforeFrom()
        {
            var sequence = Sequence.Range(0, 5);
            var index = sequence.IndexWhere(i => i == 2, from: 3);

            Assert.Equal(-1, index);
        }

        [Fact]
        public void IndexWhere_WithCount_Returns_MinusOne_When_ElementIsNotWithinRange()
        {
            var sequence = Sequence.Range(0, 5);
            var index = sequence.IndexWhere(i => i == 3, from: 0, count: 3);

            Assert.Equal(-1, index);
        }

        [Fact]
        public void LastIndexOf_Returns_IndexOfElement()
        {
            var sequence = Sequence.For(1, 1, 2, 2, 3, 3);
            var index = sequence.LastIndexOf(2);

            Assert.Equal(3, index);
        }

        [Fact]
        public void LastIndexOf_Returns_MinusOne_When_ElementIsNotFound()
        {
            var sequence = Sequence.Range(0, 5);
            var index = sequence.LastIndexOf(5);

            Assert.Equal(-1, index);
        }

        [Fact]
        public void LastIndexOf_Returns_MinusOne_When_ElementIsAfterEnd()
        {
            var sequence = Sequence.Range(0, 5);
            var index = sequence.LastIndexOf(4, end: 3);

            Assert.Equal(-1, index);
        }

        [Fact]
        public void LastIndexOf_WithCount_Returns_MinusOne_When_ElementIsNotWithinRange()
        {
            var sequence = Sequence.Range(0, 5);
            var index = sequence.LastIndexOf(1, end: 3, count: 2);

            Assert.Equal(-1, index);
        }

        [Fact]
        public void LastIndexWhere_Returns_IndexOfElement()
        {
            var sequence = Sequence.For(1, 1, 2, 2, 3, 3);
            var index = sequence.LastIndexWhere(i => i == 2);

            Assert.Equal(3, index);
        }

        [Fact]
        public void LastIndexWhere_Returns_MinusOne_When_ElementIsNotFound()
        {
            var sequence = Sequence.Range(0, 5);
            var index = sequence.LastIndexWhere(i => i == 5);

            Assert.Equal(-1, index);
        }

        [Fact]
        public void LastIndexWhere_Returns_MinusOne_When_ElementIsAfterEnd()
        {
            var sequence = Sequence.Range(0, 5);
            var index = sequence.LastIndexWhere(i => i == 4, end: 3);

            Assert.Equal(-1, index);
        }

        [Fact]
        public void LastIndexWhere_WithCount_Returns_MinusOne_When_ElementIsNotWithinRange()
        {
            var sequence = Sequence.Range(0, 5);
            var index = sequence.LastIndexWhere(i => i == 1, end: 3, count: 2);

            Assert.Equal(-1, index);
        }
    }
}
