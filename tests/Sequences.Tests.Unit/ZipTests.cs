using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class ZipTests
    {
        [Fact]
        public void Zip_AssociatesElements()
        {
            var sequence = Sequence.From(0);
            var zipped = sequence.Zip(sequence.Tail).Take(5);
            Tuple<int, int>[] expectedZipped =
                {
                    Tuple.Create(0, 1),
                    Tuple.Create(1, 2),
                    Tuple.Create(2, 3),
                    Tuple.Create(3, 4),
                    Tuple.Create(4, 5),
                };

            Assert.Equal(expectedZipped, zipped);
        }

        [Fact]
        public void Zip_TruncatesSecondSequence_If_LongerThanFirst()
        {
            var zipped = Sequence.Range(1, 4).Zip(
                Sequence.Range(10, 21));

            Tuple<int, int>[] expectedZipped =
                {
                    Tuple.Create(1, 10),
                    Tuple.Create(2, 11),
                    Tuple.Create(3, 12)
                };

            Assert.Equal(expectedZipped, zipped);
        }


        [Fact]
        public void Zip_TruncatesFirstSequence_If_LongerThanSecond()
        {
            var zipped = Sequence.Range(10, 21).Zip(
                Sequence.Range(1, 4));

            Tuple<int, int>[] expectedZipped =
                {
                    Tuple.Create(10, 1),
                    Tuple.Create(11, 2),
                    Tuple.Create(12, 3)
                };

            Assert.Equal(expectedZipped, zipped);
        }

        [Fact]
        public void ZipAll_ExtendsFirstSequence_If_ShorterThanSecond()
        {
            var zipped = Sequence.Range(1, 4).ZipAll(
                Sequence.Range(1, 6), 0, 9);

            Tuple<int, int>[] expectedZipped =
                {
                    Tuple.Create(1, 1),
                    Tuple.Create(2, 2),
                    Tuple.Create(3, 3),
                    Tuple.Create(0, 4),
                    Tuple.Create(0, 5),
                };

            Assert.Equal(expectedZipped, zipped);
        }

        [Fact]
        public void ZipAll_ExtendsSecondSequence_If_ShorterThanFirst()
        {
            var zipped = Sequence.Range(1, 6).ZipAll(
                Sequence.Range(1, 4), 0, 9);

            Tuple<int, int>[] expectedZipped =
                {
                    Tuple.Create(1, 1),
                    Tuple.Create(2, 2),
                    Tuple.Create(3, 3),
                    Tuple.Create(4, 9),
                    Tuple.Create(5, 9),
                };

            Assert.Equal(expectedZipped, zipped);
        }

        [Fact]
        public void ZipWithIndex_AssociatesElementsWithTheirIndex()
        {
            var sequence = Sequence.Range(10, 16);
            var zipped = sequence.ZipWithIndex();

            Tuple<int, int>[] expectedZipped =
                {
                    Tuple.Create(10, 0),
                    Tuple.Create(11, 1),
                    Tuple.Create(12, 2),
                    Tuple.Create(13, 3),
                    Tuple.Create(14, 4),
                    Tuple.Create(15, 5),
                };

            Assert.Equal(expectedZipped, zipped);
        }
    }
}
