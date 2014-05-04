using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class StringTests
    {
        [Fact]
        public void ToStringTest()
        {
            var seq = Sequence.Range(1, 4);
            Assert.Equal("Sequence(1, ?)", seq.ToString());
        }

        [Fact]
        public void MkString_JoinsElements()
        {
            var seq = Sequence.Range(1, 4);
            Assert.Equal("123", seq.MkString());
        }

        [Fact]
        public void MkString_JoinsElements_WithSeparator()
        {
            var seq = Sequence.Range(1, 4);
            Assert.Equal("1-2-3", seq.MkString("-"));
        }
    }
}
