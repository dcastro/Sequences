using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests.Functional
{
    public class Factorials
    {
        private readonly ISequence<int> _expectedFactorials =
            Sequence.With(1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880);

        [Fact]
        public void V1()
        {
            var naturals = Sequence.From(1);

            ISequence<int> factorials = null;
            factorials = new Sequence<int>(1,                                                           //start with (1) at index 0
                                           () => factorials.Zip(naturals)                               //zip factorials (1, ?) with naturals (1,2,3, ...)
                                                           .Select(pair => pair.Item1 * pair.Item2));   //select the product of each tuple (1,1) => 1, (1,2) => 2, (2,3) => 6
            /**
             * factorials[0] returns 1, eagerly evaluated.
             * Then, we zip factorials (1, ?) with naturals (1,2,3,...), and select the product of each tuple
             * 
             * factorials[1] returns (1,?)      zip (1,2,3,...) select(item1 * item2)[0] = factorials[0] * naturals[0] = 1 * 1 = 1
             * factorials[2] returns (1,1,?)    zip (1,2,3,...) select(item1 * item2)[1] = factorials[1] * naturals[1] = 1 * 2 = 2
             * factorials[3] returns (1,1,2,?)  zip (1,2,3,...) select(item1 * item2)[2] = factorials[2] * naturals[2] = 2 * 3 = 6
             */

            Assert.Equal(_expectedFactorials, factorials.Take(10));
        }
    }
}
