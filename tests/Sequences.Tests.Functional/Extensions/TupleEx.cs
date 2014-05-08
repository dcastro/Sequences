using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequences.Tests.Functional.Extensions
{
    public static class TupleEx
    {
        public static int Sum(this Tuple<int, int> tuple)
        {
            return tuple.Item1 + tuple.Item2;
        }

        public static int Product(this Tuple<int, int> tuple)
        {
            return tuple.Item1 * tuple.Item2;
        }
    }
}
