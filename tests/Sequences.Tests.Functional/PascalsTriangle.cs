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
            Func<ISequence<int>, ISequence<int>> func =                         //to build a row..
                row =>
                Sequence.With(1)                                                //start with 1...
                        .Concat(() =>                                           //followed by...
                                row.Zip(row.Tail)                               //zipping the previous row with its tail, i.e., (1,3,3,1) becomes ((1,3), (3,3), (3,1))
                                   .Select(pair => pair.Item1 + pair.Item2))    //and select the sum of each pair, i.e., (4, 6, 4)
                        .Append(1);                                             //and, finally, another 1.

            var triangle = Sequence.Iterate(
                Sequence.With(1), func);

            var sixRows = triangle.Take(6);

            //Assertions
            Assert.Equal(6, sixRows.Count);

            _expectedTriangle.Zip(sixRows).ForEach(
                rows => Assert.Equal(rows.Item1, rows.Item2));
        }

        [Fact]
        public void V2()
        {
            var triangle = Pascal();
            var sixRows = triangle.Take(6);

            //Assertions
            Assert.Equal(6, sixRows.Count);

            _expectedTriangle.Zip(sixRows).ForEach(
                rows => Assert.Equal(rows.Item1, rows.Item2));
        }

        private ISequence<ISequence<int>> Pascal()
        {
            ISequence<int> firstRow = Sequence.With(1);
            ISequence<int> secondRow = Sequence.With(1, 1);

            return Sequence.With(
                    firstRow,
                    secondRow
                ).Concat(() => PascalAux(secondRow));
        }

        private ISequence<ISequence<int>> PascalAux(ISequence<int> previousRow)
        {
            ISequence<int> newRow = previousRow
                .Sliding(2)                     //for each 2 consecutive elements...
                .Select(pair => pair.Sum())     //select their sum
                .AsSequence()
                .Append(1)                      //append (1)
                .Prepend(1);                    //prepend (1)
            return new Sequence<ISequence<int>>(newRow, () => PascalAux(newRow));
        }

        [Fact]
        public void V3()
        {
            //similar to V2, but doesn't define auxiliary methods
            Func<ISequence<int>, ISequence<int>> func =
                row => row.LengthCompare(1) == 0
                           ? Sequence.With(1, 1)
                           : row.Sliding(2)
                                .Select(pair => pair.Sum())
                                .AsSequence()
                                .Append(1)
                                .Prepend(1);

            var triangle = Sequence.Iterate(Sequence.With(1), func);
            var sixRows = triangle.Take(6);

            //Assertions
            Assert.Equal(6, sixRows.Count);

            _expectedTriangle.Zip(sixRows).ForEach(
                rows => Assert.Equal(rows.Item1, rows.Item2));
        }
    }
}