﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class EmptySequenceTests
    {
        [Fact]
        public void Sequence_HasNoTail()
        {
            Assert.Throws<InvalidOperationException>(() => Sequence.Empty<int>().Tail);
        }

        [Fact]
        public void Sequence_HasNoHead()
        {
            Assert.Throws<InvalidOperationException>(() => Sequence.Empty<int>().Head);
        }

        [Fact]
        public void Sequence_IsEmpty()
        {
            Assert.True(Sequence.Empty<int>().IsEmpty);
        }

        [Fact]
        public void Sequence_HasNoInit()
        {
            Assert.Throws<InvalidOperationException>(() => Sequence.Empty<int>().Init);
        }

        [Fact]
        public void ToStringTest()
        {
            Assert.Equal("Sequence()", Sequence.Empty<int>().ToString());
        }
    }
}
