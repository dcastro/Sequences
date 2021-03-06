﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class SlidingTests
    {
        [Fact]
        public void Sliding_Returns_SlidingWindows()
        {
            var sequence = Sequence.Range(0, 5);
            var windows = sequence.Sliding(3).ToList();
            var expectedWindows = new List<List<int>>
                {
                    new List<int> {0, 1, 2},
                    new List<int> {1, 2, 3},
                    new List<int> {2, 3, 4}
                };

            Assert.Equal(expectedWindows.Count, windows.Count);

            for (int i = 0; i < windows.Count; i++)
                Assert.Equal(expectedWindows[i], windows[i]);
        }

        [Fact]
        public void Sliding_WithStep_TruncatesLastWindow()
        {
            var sequence = Sequence.Range(0, 6);
            var windows = sequence.Sliding(3, 2).ToList();
            var expectedWindows = new List<List<int>>
                {
                    new List<int> {0, 1, 2},
                    new List<int> {2, 3, 4},
                    new List<int> {4, 5}
                };

            Assert.Equal(expectedWindows.Count, windows.Count);

            for (int i = 0; i < windows.Count; i++)
                Assert.Equal(expectedWindows[i], windows[i]);
        }

        [Fact]
        public void Sliding_Stops_When_LastElementIsSkipped()
        {
            var sequence = Sequence.Range(0, 8);
            var windows = sequence.Sliding(2, 4).ToList();
            var expectedWindows = new List<List<int>>
                {
                    new List<int> {0, 1},
                    new List<int> {4, 5}
                };

            Assert.Equal(expectedWindows.Count, windows.Count);

            for (int i = 0; i < windows.Count; i++)
                Assert.Equal(expectedWindows[i], windows[i]);
        }

        [Fact]
        public void Sliding_Returns_WholeSequence_When_SizeIsHigherThanCount()
        {
            var sequence = Sequence.Range(0, 4);
            var windows = sequence.Sliding(10).ToList();
            var expectedWindow = new List<int> {0, 1, 2, 3};

            Assert.Equal(1, windows.Count);
            Assert.Equal(expectedWindow, windows.Single());
        }

        [Fact]
        public void Sliding_Returns_EmptyEnumerable_When_SequenceIsEmpty()
        {
            var sequence = Sequence.Empty<int>();
            var windows = sequence.Sliding(10);

            Assert.False(windows.Any());
        }

        [Fact]
        public void Sliding_ThrowsException_When_SizeIsNotPositive()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Sequence.Empty<int>().Sliding(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => Sequence.Empty<int>().Sliding(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => Sequence.Empty<int>().Sliding(0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => Sequence.Empty<int>().Sliding(-1, 1));
        }

        [Fact]
        public void Sliding_ThrowsException_When_StepIsNotPositive()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Sequence.Empty<int>().Sliding(1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => Sequence.Empty<int>().Sliding(1, -1));
        }
    }
}
