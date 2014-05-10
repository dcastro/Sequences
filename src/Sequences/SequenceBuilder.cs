using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sequences
{
    /// <summary>
    /// A mutable builder, capable of building sequences from elements and other collections.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the resulting sequences.</typeparam>
    public class SequenceBuilder<T>
    {
        private readonly List<Func<IEnumerable<T>>> _parts = new List<Func<IEnumerable<T>>>();

        /// <summary>
        /// Appends one or more elements to this builder.
        /// </summary>
        /// <param name="elems">One or more elements to be added to this builder.</param>
        /// <returns>This builder.</returns>
        public SequenceBuilder<T> Append(params T[] elems)
        {
            return Append(elems.AsEnumerable());
        }

        /// <summary>
        /// Appends a collection to this builder.
        /// </summary>
        /// <param name="enumerable">The collection to be added to this builder.</param>
        /// <returns>This builder.</returns>
        public SequenceBuilder<T> Append(IEnumerable<T> enumerable)
        {
            if (enumerable == null) throw new ArgumentNullException("enumerable");

            return Append(() => enumerable);
        }

        /// <summary>
        /// Appends a lazily-evaluated collection to this builder.
        /// </summary>
        /// <param name="enumerable">The collection to be added to this builder.</param>
        /// <returns>This builder.</returns>
        public SequenceBuilder<T> Append(Func<IEnumerable<T>> enumerable)
        {
            if (enumerable == null) throw new ArgumentNullException("enumerable");

            _parts.Add(enumerable);
            return this;
        }

        /// <summary>
        /// Appends a single element to this builder.
        /// </summary>
        /// <param name="elem">The element to be added to this builder.</param>
        /// <returns>This builder.</returns>
        public SequenceBuilder<T> Append(T elem)
        {
            return Append(() => elem);
        }

        /// <summary>
        /// Appends a single lazily-evaluated element to this builder.
        /// </summary>
        /// <param name="elem">The element to be added to this builder.</param>
        /// <returns>This builder.</returns>
        public SequenceBuilder<T> Append(Func<T> elem)
        {
            _parts.Add(() => Sequence.With(elem()));
            return this;
        }

        /// <summary>
        /// Produces a sequence from the added elements.
        /// </summary>
        /// <returns>A sequence containing the elements added to this builder.</returns>
        public ISequence<T> ToSequence()
        {
            //realize sublist and flatten it as a sequence
            return _parts
                .Take(_parts.Count)
                .ToList()
                .SelectMany(enumerable => enumerable())
                .AsSequence();
        }

        /// <summary>
        /// Clears the contents of this builder.
        /// </summary>
        /// <returns>This builder.</returns>
        public SequenceBuilder<T> Clear()
        {
            _parts.Clear();
            return this;
        }
    }
}
