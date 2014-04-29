using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Sequences
{
    public partial class Sequence<T>
    {
        private class SlidingIterator : IEnumerable<ISequence<T>>
        {
            private readonly ISequence<T> _sequence;
            private readonly int _size;
            private readonly int _step;

            public SlidingIterator(ISequence<T> sequence, int size, int step)
            {
                _sequence = sequence;
                _size = size;
                _step = step;
            }

            public IEnumerator<ISequence<T>> GetEnumerator()
            {
                ISequence<T> seq = _sequence;
                var buffer = new List<T>(_size);

                bool hasMoreElems = !seq.IsEmpty;

                while (hasMoreElems)
                {
                    //group elements into a buffer
                    IEnumerator<T> iterator = seq.GetEnumerator();

                    for (int i = 0; i < _size && iterator.MoveNext(); i++)
                        buffer.Add(iterator.Current);

                    //force the evaluation of the buffer's contents, before we clear the buffer.
                    yield return buffer.AsSequence().Force();

                    //keep going if there's at least one more element
                    hasMoreElems = iterator.MoveNext();
                    buffer.Clear();
                    seq = seq.Skip(_step);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class GroupedIterator : IEnumerable<ISequence<T>>
        {
            private readonly ISequence<T> _sequence;
            private readonly int _size;

            public GroupedIterator(ISequence<T> sequence, int size)
            {
                _sequence = sequence;
                _size = size;
            }

            public IEnumerator<ISequence<T>> GetEnumerator()
            {
                ISequence<T> seq = _sequence;

                while (!seq.IsEmpty)
                {
                    yield return seq.Take(_size);
                    seq = seq.Skip(_size);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class CombinationsIterator : IEnumerable<ISequence<T>>
        {
            private readonly ISequence<T> _sequence;
            private readonly int _size;

            public CombinationsIterator(ISequence<T> sequence, int size)
            {
                _sequence = sequence;
                _size = size;
            }

            public IEnumerator<ISequence<T>> GetEnumerator()
            {
                if (_size == 0)
                    yield return Sequence.Empty<T>();
                else
                    //combine each distinct element (subsequence.Head)
                    //with each possible combination of the remaining elements (tailCombination)
                    foreach (var subSequence in _sequence.NonEmptyTails().Distinct(new CompareByHead()))
                        foreach (var tailCombination in subSequence.Tail.Combinations(_size - 1))
                            yield return new Sequence<T>(subSequence.Head, () => tailCombination);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <summary>
            /// Compares two sequences by their heads. The implementation assumes input sequences are not empty.
            /// </summary>
            private class CompareByHead : IEqualityComparer<ISequence<T>>
            {
                private readonly IEqualityComparer<T> _headComparer = EqualityComparer<T>.Default;

                public bool Equals(ISequence<T> x, ISequence<T> y)
                {
                    return _headComparer.Equals(x.Head, y.Head);
                }

                public int GetHashCode(ISequence<T> seq)
                {
                    return seq.Head.GetHashCode();
                }
            }
        }

        private class PermutationsIterator : IEnumerable<ISequence<T>>
        {
            private readonly ISequence<T> _sequence;

            public PermutationsIterator(ISequence<T> sequence)
            {
                _sequence = sequence;
            }

            public IEnumerator<ISequence<T>> GetEnumerator()
            {
                if (_sequence.IsEmpty)
                    yield return Sequence.Empty<T>();
                else
                    foreach (var elem in _sequence.Distinct())
                        foreach (var restPermutation in _sequence.Remove(elem).Permutations())
                            yield return new Sequence<T>(elem, () => restPermutation);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
