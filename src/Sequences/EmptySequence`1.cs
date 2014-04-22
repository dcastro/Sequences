using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sequences
{
    internal class EmptySequence<T> : Sequence<T>
    {
        private static readonly Lazy<ISequence<T>> Empty = new Lazy<ISequence<T>>(() => new EmptySequence<T>());

        private EmptySequence() : base(default(T), null as Lazy<ISequence<T>>)
        {
        }

        public static ISequence<T> Instance
        {
            get { return Empty.Value; }
        }

        public override bool IsEmpty
        {
            get { return true; }
        }

        public override T Head
        {
            get { throw new InvalidOperationException("An empty sequence doesn't have a head."); }
        }

        public override ISequence<T> Tail
        {
            get { throw new InvalidOperationException("An empty sequence doesn't have a tail."); }
        }
    }
}
