using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Sequences.Tests
{
    public class TailTests
    {

        [Fact]
        public void Tail_Is_LazilyEvaluated()
        {
            //Arrange
            var tailMock = new Mock<Func<ISequence<int>>>();
            tailMock.Setup(tail => tail()).Returns(Sequence<int>.Empty);

            var sequence = new Sequence<int>(1, tailMock.Object);

            //Act & Assert
            tailMock.Verify(tail => tail(), Times.Never);

            Assert.Equal(Sequence<int>.Empty, sequence.Tail);
            tailMock.Verify(tail => tail(), Times.Once);
        }
    }
}
