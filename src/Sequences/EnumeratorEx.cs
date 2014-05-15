using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sequences
{
    internal static class EnumeratorEx
    {
        /// <summary>
        /// Call MoveNext, and dispose of the iterator if MoveNext returns false or throws an exception.
        /// </summary>
        public static bool TryMoveNext<T>(this IEnumerator<T> iterator)
        {
            try
            {
                bool moved = iterator.MoveNext();

                if (!moved)
                    iterator.Dispose();

                return moved;
            }
            catch (Exception)
            {
                if (iterator != null)
                    iterator.Dispose();

                throw;
            }
        }
    }
}
