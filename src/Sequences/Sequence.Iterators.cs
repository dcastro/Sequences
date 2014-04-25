using System;
using System.Collections;
using System.Collections.Generic;
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
    }
}
