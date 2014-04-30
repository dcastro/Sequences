using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Sequences.Tests
{
    public class EnumerableTests
    {
        [Fact]
        public void Enumerator_IteratesThroughElements()
        {
            var sequence = new Sequence<int>(1, () =>
                                new Sequence<int>(2, () =>
                                    new Sequence<int>(3, Sequence.Empty<int>)));

            Assert.Equal(new []{1,2,3}, sequence);
        }

        [Fact]
        public void ForEach_AppliesFunctionToEachElement()
        {
            var funcMock = new Mock<Action<int>>();
            var sequence = Sequence.Range(1,4);

            sequence.ForEach(funcMock.Object);

            funcMock.Verify(f => f(It.IsAny<int>()), Times.Exactly(3));

            funcMock.Verify(f => f(1), Times.Exactly(1));
            funcMock.Verify(f => f(2), Times.Exactly(1));
            funcMock.Verify(f => f(3), Times.Exactly(1));
        }
    }
}
