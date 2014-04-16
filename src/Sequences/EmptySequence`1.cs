using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sequences
{
    internal class EmptySequence<T> : ISequence<T>
    {
        private static readonly Lazy<ISequence<T>> Empty = new Lazy<ISequence<T>>(() => new EmptySequence<T>());

        private EmptySequence()
        {
        }

        public static ISequence<T> Instance
        {
            get { return Empty.Value; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool IsEmpty
        {
            get { return true; }
        }

        public T Head
        {
            get { throw new InvalidOperationException("An empty sequence doesn't have a head."); }
        }

        public ISequence<T> Tail
        {
            get { throw new InvalidOperationException("An empty sequence doesn't have a tail."); }
        }
    }
}
