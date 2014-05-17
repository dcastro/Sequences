using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class KmpSearchTests
    {
        private const int SourcesCount = 10000;
        private const int SourceLength = 1000;

        private const int WordMinLength = 2;
        private const int WordMaxLength = 10;

        private const string Chars = "0123456789";
        private readonly Random _rnd = new Random();

        [Fact]
        public void Kmp_FindsFirstIndexOfWord()
        {
            foreach (var data in GetTestData())
            {
                //with unknown size
                int strSearchResult = data.Source.IndexOf(data.Word, data.From, StringComparison.InvariantCulture);
                int seqSearchResult = data.Source.AsSequence().IndexOfSlice(data.Word, data.From);

                Assert.Equal(strSearchResult, seqSearchResult);

                //with known size
                int forcedSeqSearchResult = data.Source.AsSequence().Force().IndexOfSlice(data.Word, data.From);
                Assert.Equal(strSearchResult, forcedSeqSearchResult);
            }
        }

        [Fact]
        public void EdgeCases()
        {
            //source is empty + word is empty
            Assert.Equal(0, Sequence.Empty<int>().IndexOfSlice(new int[] {}));
            Assert.Equal(0, Sequence.Empty<int>().IndexOfSlice(new int[] {}, -1));
            Assert.Equal(-1, Sequence.Empty<int>().IndexOfSlice(new int[] {}, 1));

            //word is empty
            Assert.Equal(0, Sequence.With(1, 2).IndexOfSlice(new int[] {}));
            Assert.Equal(0, Sequence.With(1, 2).IndexOfSlice(new int[] {}, -1));
            Assert.Equal(1, Sequence.With(1, 2).IndexOfSlice(new int[] {}, 1));
            Assert.Equal(-1, Sequence.With(1, 2).IndexOfSlice(new int[] {}, 2));

            //source == word
            Assert.Equal(0, Sequence.With(1, 2, 3).IndexOfSlice(new[] {1, 2, 3}));
            Assert.Equal(0, Sequence.With(1, 2, 3).IndexOfSlice(new[] {1, 2, 3}, -1));
            Assert.Equal(-1, Sequence.With(1, 2, 3).IndexOfSlice(new[] {1, 2, 3}, 1));

            Assert.Equal(0, Sequence.With(1, 2, 3).Force().IndexOfSlice(new[] {1, 2, 3}));
            Assert.Equal(0, Sequence.With(1, 2, 3).Force().IndexOfSlice(new[] {1, 2, 3}, -1));
            Assert.Equal(-1, Sequence.With(1, 2, 3).Force().IndexOfSlice(new[] {1, 2, 3}, 1));

            //word.Length == 1
            Assert.Equal(1, Sequence.With(1, 2).Force().IndexOfSlice(new[] {2}));
            Assert.Equal(1, Sequence.With(1, 2).Force().IndexOfSlice(new[] {2}, -1));
            Assert.Equal(0, Sequence.With(1, 2).Force().IndexOfSlice(new[] {2}, 1));
            Assert.Equal(-1, Sequence.With(1, 2).Force().IndexOfSlice(new[] {2}, 2));
        }

        private IEnumerable<TestData> GetTestData()
        {
            for (int i = 0; i < SourcesCount; i++)
            {
                char[] sourceChars = Enumerable.Repeat(Chars, SourceLength)
                                               .Select(chars => chars[_rnd.Next(chars.Length)])
                                               .ToArray();

                int length = _rnd.Next(WordMinLength, WordMaxLength);
                int start = _rnd.Next(sourceChars.Length - length);

                var source = new string(sourceChars);
                var word = source.Substring(start, length);

                var from = _rnd.Next(0, start + 1);

                yield return new TestData
                    {
                        Source = source,
                        Word = word,
                        From = from
                    };
            }
        }

        private class TestData
        {
            public string Source;
            public string Word;
            public int From;
        }
    }
}
