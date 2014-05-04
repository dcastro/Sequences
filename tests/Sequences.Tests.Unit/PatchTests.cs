using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class PatchTests
    {
        [Fact]
        public void Patch_ReplacesWithPatch()
        {
            var sequence = Sequence.Range(0, 6);
            var patched = sequence.Patch(2, new[] {9, 9}, 2);
            int[] expectedPatched = {0, 1, 9, 9, 4, 5};

            Assert.Equal(expectedPatched, patched);
        }

        [Fact]
        public void Patch_ErasesOriginalElements()
        {
            var sequence = Sequence.Range(0, 6);
            var patched = sequence.Patch(2, new int[] {}, 2);
            int[] expectedPatched = {0, 1, 4, 5};

            Assert.Equal(expectedPatched, patched);
        }

        [Fact]
        public void Patch_ConcatenatesPatch_When_FromIsEqualToCount()
        {
            var sequence = Sequence.Range(0, 4);
            var patched = sequence.Patch(4, new[] {9, 9}, 2);
            int[] expectedPatched = {0, 1, 2, 3, 9, 9};

            Assert.Equal(expectedPatched, patched);
        }

        [Fact]
        public void Patch_ConcatenatesPatch_When_FromIsGreaterThanCount()
        {
            var sequence = Sequence.Range(0, 4);
            var patched = sequence.Patch(10, new[] {9, 9}, 2);
            int[] expectedPatched = {0, 1, 2, 3, 9, 9};

            Assert.Equal(expectedPatched, patched);
        }
    }
}
