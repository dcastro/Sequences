﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class AggregationTests
    {
        [Fact]
        public void Fold_AccumulatesValues()
        {
            int sum = Sequence.With(1, 2, 3, 4).Fold(0, (a, b) => a + b);
            Assert.Equal(10, sum);
        }

        [Fact]
        public void Fold_ReturnsSeed_When_SequenceIsEmpty()
        {
            int sum = Sequence.Empty<int>().Fold(0, (a, b) => a + b);
            Assert.Equal(0, sum);
        }

        [Fact]
        public void Fold_GoesLeftToRight()
        {
            var additions = new List<Tuple<int, int>>();
            var expectedAdditions = new List<Tuple<int, int>>
                {
                    Tuple.Create(0, 1),
                    Tuple.Create(1, 2),
                    Tuple.Create(3, 3),
                    Tuple.Create(6, 4)
                };

            Sequence.With(1, 2, 3, 4).Fold(0, (a, b) =>
                {
                    additions.Add(Tuple.Create(a, b));
                    return a + b;
                });

            Assert.Equal(expectedAdditions, additions);
        }

        [Fact]
        public void FoldRight_AccumulatesValues()
        {
            int sum = Sequence.With(1, 2, 3, 4).FoldRight(0, (a, b) => a + b);
            Assert.Equal(10, sum);
        }

        [Fact]
        public void FoldRight_ReturnsSeed_When_SequenceIsEmpty()
        {
            int sum = Sequence.Empty<int>().FoldRight(0, (a, b) => a + b);
            Assert.Equal(0, sum);
        }

        [Fact]
        public void FoldRight_GoesRightToLeft()
        {

            var additions = new List<Tuple<int, int>>();
            var expectedAdditions = new List<Tuple<int, int>>
                {
                    Tuple.Create(4, 0),
                    Tuple.Create(3, 4),
                    Tuple.Create(2, 7),
                    Tuple.Create(1, 9)
                };

            Sequence.With(1, 2, 3, 4).FoldRight(0, (a, b) =>
                {
                    additions.Add(Tuple.Create(a, b));
                    return a + b;
                });

            Assert.Equal(expectedAdditions, additions);
        }

        [Fact]
        public void Reduce_AccumulatesValues()
        {
            int sum = Sequence.With(1, 2, 3, 4).Reduce((a, b) => a + b);
            Assert.Equal(10, sum);
        }

        [Fact]
        public void Reduce_Throws_When_SequenceIsEmpty()
        {
            Assert.Throws<InvalidOperationException>(() => Sequence.Empty<int>().Reduce((a, b) => a + b));
        }

        [Fact]
        public void ReduceRight_AccumulatesValues()
        {
            int sum = Sequence.With(1, 2, 3, 4).ReduceRight((a, b) => a + b);
            Assert.Equal(10, sum);
        }

        [Fact]
        public void ReduceRight_Throws_When_SequenceIsEmpty()
        {
            Assert.Throws<InvalidOperationException>(() => Sequence.Empty<int>().ReduceRight((a, b) => a + b));
        }

        [Fact]
        public void Scan_Returns_ListOfAccumulations()
        {
            var sequence = Sequence.With(1, 2, 3, 4);
            var scan = sequence.Scan(0, (a, b) => a + b);
            var expected = new[] {0, 1, 3, 6, 10};

            Assert.Equal(expected, scan);
        }

        [Fact]
        public void Scan_Returns_Seed_When_SequenceIsEmpty()
        {
            var sequence = Sequence.Empty<int>();
            var scan = sequence.Scan(0, (a, b) => a + b);
            var expected = new[] {0};

            Assert.Equal(expected, scan);
        }

        [Fact]
        public void ScanRight_Returns_ListOfAccumulations()
        {
            var sequence = Sequence.With(1, 2, 3, 4);
            var scan = sequence.ScanRight(0, (a, b) => a + b);
            var expected = new[] {10, 9, 7, 4, 0};

            Assert.Equal(expected, scan);
        }

        [Fact]
        public void ScanRight_Returns_Seed_When_SequenceIsEmpty()
        {
            var sequence = Sequence.Empty<int>();
            var scan = sequence.ScanRight(0, (a, b) => a + b);
            var expected = new[] {0};

            Assert.Equal(expected, scan);
        }
    }
}
