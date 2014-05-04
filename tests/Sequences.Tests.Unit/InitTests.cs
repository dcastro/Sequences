using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class InitTests
    {
        [Fact]
        public void Init_ExcludesLastElement()
        {
            var sequence = Sequence.Range(1, 6);
            var init = sequence.Init;
            int[] expectedInit = {1, 2, 3, 4};

            Assert.Equal(expectedInit, init);
        }

        [Fact]
        public void Inits_IteratesOverInits()
        {
            var sequence = Sequence.Range(1, 4);
            var inits = sequence.Inits().ToList();
            var expectedInits = new[]
            {
                new[] {1, 2, 3},
                new[] {1, 2},
                new[] {1},
                new int[] {}
            };

            Assert.Equal(expectedInits.Length, inits.Count);

            for (int i = 0; i < inits.Count; i++)
                Assert.Equal(expectedInits[i], inits[i]);
        }
    }
}
