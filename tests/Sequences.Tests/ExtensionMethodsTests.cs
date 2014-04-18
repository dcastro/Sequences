using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class ExtensionMethodsTests
    {
        private readonly IEnumerable<int> _enumerable;
        private readonly ISequence<int> _sequence;

        public ExtensionMethodsTests()
        {
            _enumerable = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10};
            _sequence = _enumerable.AsSequence();
        }

        [Fact]
        public void Concat_ThrowsException_When_FirstIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => (null as Sequence<int>).Concat(
                    Sequence.Empty<int>));
        }

        [Fact]
        public void Concat_ThrowsException_When_SecondIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => Sequence.Empty<int>().Concat(
                    () => (null as Sequence<int>)));
        }

        [Fact]
        public void Concat_ThrowsException_When_SecondFunctionIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => Sequence.Empty<int>().Concat(
                    null as Func<ISequence<int>>));
        }

        [Fact]
        public void Concat_ConcatenatesInputSequences()
        {
            var first = Sequence.For(1, 2, 3);
            var second = Sequence.For(4, 5, 6);

            Assert.Equal(new[] {1, 2, 3, 4, 5, 6}, first.Concat(() => second));
        }

        [Fact]
        public void Select()
        {
            Assert.Equal(_enumerable.Select(i => i + 1),
                         _sequence.Select(i => i + 1));
        }

        [Fact]
        public void Select_With_Index()
        {
            Assert.Equal(_enumerable.Select((i, index) => i + index),
                         _sequence.Select((i, index) => i + index));
        }

        [Fact]
        public void SelectMany()
        {
            Assert.Equal(_enumerable.SelectMany(i => new[] {i, i + 1}),
                         _sequence.SelectMany(i => new[] {i, i + 1}));
        }

        [Fact]
        public void SelectMany_With_Index()
        {
            Assert.Equal(_enumerable.SelectMany((i, index) => new[] {i, index}),
                         _sequence.SelectMany((i, index) => new[] {i, index}));
        }

        [Fact]
        public void SelectMany_With_IntermediateCollection()
        {
            Assert.Equal(_enumerable.SelectMany(i => new[] {i, i + 1},
                                                (i1, i2) => i1 + i2),
                         _sequence.SelectMany(i => new[] {i, i + 1},
                                              (i1, i2) => i1 + i2));
        }

        [Fact]
        public void SelectMany_With_IntermediateCollection_With_Index()
        {
            Assert.Equal(_enumerable.SelectMany((i, index) => new[] {i, index},
                                                (i1, i2) => i1 + i2),
                         _sequence.SelectMany((i, index) => new[] {i, index},
                                              (i1, i2) => i1 + i2));
        }

        [Fact]
        public void Where()
        {
            Assert.Equal(_enumerable.Where(i => i%2 == 0),
                         _sequence.Where(i => i%2 == 0));
        }

        [Fact]
        public void Where_With_Index()
        {
            Assert.Equal(_enumerable.Where((i, index) => i + index%2 == 0),
                         _sequence.Where((i, index) => i + index%2 == 0));
        }

        [Fact]
        public void Skip()
        {
            Assert.Equal(_enumerable.Skip(3),
                         _sequence.Skip(3));
        }

        [Fact]
        public void SkipWhile()
        {
            Assert.Equal(_enumerable.SkipWhile(i => i < 5),
                         _sequence.SkipWhile(i => i < 5));
        }

        [Fact]
        public void SkipWhile_With_Index()
        {
            Assert.Equal(_enumerable.SkipWhile((i, index) => index < 5),
                         _sequence.SkipWhile((i, index) => index < 5));
        }

        [Fact]
        public void Take()
        {
            Assert.Equal(_enumerable.Take(3),
                         _sequence.Take(3));
        }

        [Fact]
        public void TakeWhile()
        {
            Assert.Equal(_enumerable.TakeWhile(i => i < 5),
                         _sequence.TakeWhile(i => i < 5));
        }

        [Fact]
        public void TakeWhile_With_Index()
        {
            Assert.Equal(_enumerable.TakeWhile((i, index) => index < 5),
                         _sequence.TakeWhile((i, index) => index < 5));
        }

        [Fact]
        public void Zip()
        {
            Assert.Equal(_enumerable.Zip(_enumerable.Skip(1), Tuple.Create),
                         _sequence.Zip(_sequence.Tail));
        }

        [Fact]
        public void Zip_With_Selector()
        {
            Assert.Equal(_enumerable.Zip(_enumerable.Skip(1), Tuple.Create),
                         _sequence.Zip(_sequence.Tail, Tuple.Create));
        }

        [Fact]
        public void Distinct()
        {
            Assert.Equal(_enumerable.Distinct(),
                         _sequence.Distinct());
        }

        [Fact]
        public void Distinct_With_Comparer()
        {
            Assert.Equal(_enumerable.Distinct(new SillyIntComparer()),
                         _sequence.Distinct(new SillyIntComparer()));
        }

        [Fact]
        public void Except()
        {
            Assert.Equal(_enumerable.Except(new[] {1, 2, 3}),
                         _sequence.Except(new[] {1, 2, 3}));
        }

        [Fact]
        public void Except_With_Comparer()
        {
            Assert.Equal(_enumerable.Except(new[] {1, 2, 3}, new SillyIntComparer()),
                         _sequence.Except(new[] {1, 2, 3}, new SillyIntComparer()));
        }

        [Fact]
        public void Intersect()
        {
            Assert.Equal(_enumerable.Intersect(new[] {9, 10, 11, 12}),
                         _sequence.Intersect(new[] {9, 10, 11, 12}));
        }

        [Fact]
        public void Intersect_With_Comparer()
        {
            Assert.Equal(_enumerable.Intersect(new[] {9, 10, 11, 12}, new SillyIntComparer()),
                         _sequence.Intersect(new[] {9, 10, 11, 12}, new SillyIntComparer()));
        }

        [Fact]
        public void Reverse()
        {
            Assert.Equal(_enumerable.Reverse(), _sequence.Reverse());
        }

        [Fact]
        public void Union()
        {
            Assert.Equal(_enumerable.Union(new[] {9, 10, 11, 12}),
                         _sequence.Union(new[] {9, 10, 11, 12}));
        }

        [Fact]
        public void Union_With_Comparer()
        {
            Assert.Equal(_enumerable.Union(new[] {9, 10, 11, 12}, new SillyIntComparer()),
                         _sequence.Union(new[] {9, 10, 11, 12}, new SillyIntComparer()));
        }

        [Fact]
        public void SumBigInteger()
        {
            Assert.Equal(10, Sequence.For<BigInteger>(0, 2, 5, 3).Sum());
        }

        [Fact]
        public void SumBigInteger_With_Selector()
        {
            Assert.Equal(10, Sequence.For("", "hello", "world")
                                     .Sum(str => (BigInteger) str.Length));
        }

        [Fact]
        public void SumNullableBigInteger()
        {
            Assert.Equal(10, Sequence.For<BigInteger?>(null, 0, null, 2, 5, 3, null).Sum());
        }

        [Fact]
        public void SumNullableIntegers_WithNoValues()
        {
            Assert.Equal(new int?[] {null}.Sum(),
                         new BigInteger?[] {null}.AsSequence().Sum());
        }

        [Fact]
        public void SumNullableBigInteger_With_Selector()
        {
            Assert.Equal(10, Sequence.For(null, "", null, "hello", "world", null)
                                     .Sum(str => str == null ? null : (BigInteger?) str.Length));
        }

        private class SillyIntComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return true;
            }

            public int GetHashCode(int obj)
            {
                return 1;
            }
        }
    }
}
