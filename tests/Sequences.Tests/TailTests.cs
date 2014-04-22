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
            tailMock.Setup(tail => tail()).Returns(Sequence.Empty<int>());

            var sequence = new Sequence<int>(1, tailMock.Object);

            //Act & Assert
            tailMock.Verify(tail => tail(), Times.Never);

            Assert.Equal(Sequence.Empty<int>(), sequence.Tail);
            tailMock.Verify(tail => tail(), Times.Once);
        }

        [Fact]
        public void IsTailDefined_ReturnsFalse_When_TailHasntBeenEvaluated()
        {
            var sequence = new Sequence<int>(1, Sequence.Empty<int>);
            Assert.False(sequence.IsTailDefined);
        }

        [Fact]
        public void IsTailDefined_ReturnsTrue_When_TailHasBeenEvaluated()
        {
            var sequence = new Sequence<int>(1, Sequence.Empty<int>);
            var tail = sequence.Tail;
            Assert.True(sequence.IsTailDefined);
        }

        [Fact]
        public void Force_EvaluatesTail()
        {
            var sequence = Sequence.Range(0, 6, 1);
            Assert.False(sequence.HasDefiniteSize);

            sequence.Force();
            Assert.True(sequence.HasDefiniteSize);
        }
    }
}
