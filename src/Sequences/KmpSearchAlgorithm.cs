using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sequences
{
    /// <summary>
    /// Searches for a subsequence within a sequence using a variation of the Knuth-Morris-Pratt algorithm.
    /// This variation doesn't require that we know the size of "source" beforehand.
    /// </summary>
    internal static class KmpSearchAlgorithm
    {
        /// <summary>
        /// Searches for a subsequence within a sequence using a variation of the Knuth-Morris-Pratt algorithm.
        /// This variation doesn't require that we know the size of "source" beforehand.
        /// </summary>
        internal static int Search<T>(IEnumerable<T> source, T[] word, IEqualityComparer<T> cmp = null)
        {
            if (cmp == null)
                cmp = EqualityComparer<T>.Default;

            using (var iterator = source.GetEnumerator())
                return Search(iterator, word, cmp);
        }

        /**
         * Assumes word.Length >= 2
         */

        private static int Search<T>(IEnumerator<T> source, T[] word, IEqualityComparer<T> cmp)
        {
            var table = KmpTable(word, cmp);

            int wordIndex = 0;
            int index = 0;

            //if the source is empty
            if (!source.MoveNext())
                return -1;

            while (true)
            {
                var e = source.Current;

                if (cmp.Equals(e, word[wordIndex]))
                {
                    if (wordIndex == word.Length - 1)
                        return (index - word.Length + 1);

                    wordIndex++;
                }
                else
                {
                    int iTemp = wordIndex;
                    wordIndex = table[wordIndex];

                    //if an ongoing match just failed,
                    //compare "e" again, but this time against word[table[i]]
                    if (iTemp > 0)
                        continue;
                }

                index++;
                if (!source.MoveNext())
                    break;
            }
            return -1;
        }

        /// <summary>
        /// Builds the "partial match" table (or jump table).
        /// Table[i] represents how characters of "word" can be skipped, when the comparison between "e" and word[i] fails
        /// </summary>
        private static int[] KmpTable<T>(T[] word, IEqualityComparer<T> cmp)
        {
            var t = new int[word.Length];

            t[0] = 0;
            t[1] = 0;

            int pos = 2;
            int cnd = 0;

            while (pos < word.Length)
            {
                if (cmp.Equals(word[pos - 1], word[cnd]))
                {
                    t[pos] = cnd + 1;
                    cnd++;
                    pos++;
                }
                else if (cnd > 0)
                {
                    cnd = t[cnd];
                }
                else
                {
                    t[pos] = 0;
                    pos++;
                }
            }

            return t;
        }
    }
}
