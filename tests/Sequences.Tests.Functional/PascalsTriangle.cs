using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests.Functional
{
    public class PascalsTriangle
    {
        private readonly ISequence<ISequence<int>> _expectedTriangle;

        public PascalsTriangle()
        {
            _expectedTriangle = Sequence.With(Sequence.With(1),
                                              Sequence.With(1, 1),
                                              Sequence.With(1, 2, 1),
                                              Sequence.With(1, 3, 3, 1),
                                              Sequence.With(1, 4, 6, 4, 1),
                                              Sequence.With(1, 5, 10, 10, 5, 1));
        }

        [Fact]
        public void V1()
        {
            var triangle = Sequence.Iterate(
                Sequence.With(1),                                               //begin with row (1)
                row =>
                Sequence.With(1)                                                //and, build the next row starting with 1...
                        .Concat(() =>                                           //followed by...
                                row.Zip(row.Tail)                               //zipping the previous row with its tail, i.e., (1,3,3,1) becomes ((1,3), (3,3), (3,1))
                                   .Select(pair => pair.Item1 + pair.Item2))    //and select the sum of each pair, i.e., (4, 6, 4)
                        .Append(1));                                            //and, finally, another 1.

            var sixRows = triangle.Take(6);

            sixRows.ForEach(row => Console.WriteLine(row.MkString(" ")));

            //Assert
            Assert.Equal(6, sixRows.Count);

            sixRows.Zip(_expectedTriangle).ForEach(
                rows => Assert.Equal(rows.Item1, rows.Item2));
        }
    }
}
