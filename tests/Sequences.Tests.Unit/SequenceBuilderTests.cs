using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Sequences.Tests
{
    public class SequenceBuilderTests
    {
        [Fact]
        public void ResultSequence_IncludesAppendedCollections()
        {
            var collection1 = new List<int> {1, 2, 3};
            var collection2 = Sequence.Range(4, 7);

            var builder = Sequence.NewBuilder<int>()
                                  .Append(collection1)
                                  .Append(collection2);

            Assert.Equal(new[] {1, 2, 3, 4, 5, 6}, builder.ToSequence());
        }

        [Fact]
        public void ResultSequence_IncludesAppendedLazyCollections()
        {
            var collection1 = new List<int> {1, 2, 3};
            var collection2 = Sequence.Range(4, 7);

            var builder = Sequence.NewBuilder<int>()
                                  .Append(() => collection1)
                                  .Append(() => collection2);

            Assert.Equal(new[] {1, 2, 3, 4, 5, 6}, builder.ToSequence());
        }

        [Fact]
        public void ResultSequence_IncludesAppendedElements()
        {
            var builder = Sequence.NewBuilder<int>()
                                  .Append(1)
                                  .Append(2, 3);

            Assert.Equal(new[] {1, 2, 3}, builder.ToSequence());
        }

        [Fact]
        public void ResultSequence_IncludesAppendedLazyElements()
        {
            var builder = Sequence.NewBuilder<int>()
                                  .Append(() => 1);

            Assert.Equal(new[] {1}, builder.ToSequence());
        }

        [Fact]
        public void ResultSequence_LazilyEvaluates_AppendedCollectionsContents()
        {
            var inputSequence = Sequence.Range(4, 7);

            var builder = Sequence.NewBuilder<int>()
                                  .Append(inputSequence);

            var resultSequence = builder.ToSequence();

            //assert that the input sequence hasn't been fully realized yet
            Assert.False(inputSequence.IsTailDefined);
        }

        [Fact]
        public void ResultSequence_LazilyEvaluates_AppendedCollectionsAndElements()
        {
            //setup mocks for lazy elements/colection
            var lazyElemMock1 = new Mock<Func<int>>();
            lazyElemMock1.Setup(elem => elem()).Returns(1);

            var lazyElemMock2 = new Mock<Func<int>>();
            lazyElemMock2.Setup(elem => elem()).Returns(2);

            var lazyCollectionMock = new Mock<Func<IEnumerable<int>>>();
            lazyCollectionMock.Setup(xs => xs()).Returns(new[] {3, 4, 5});

            var builder = Sequence.NewBuilder<int>()
                                  .Append(lazyElemMock1.Object)
                                  .Append(lazyElemMock2.Object)
                                  .Append(lazyCollectionMock.Object);

            var iter = builder.ToSequence().GetEnumerator();

            //head is eagerly evaluated - all other members shouldn't have been evaluated yet.
            lazyElemMock1.Verify(e => e(), Times.Once);
            lazyElemMock2.Verify(e => e(), Times.Never);
            lazyCollectionMock.Verify(xs => xs(), Times.Never);

            //move to the 1st element
            iter.MoveNext();

            //state shouldn't have changed
            lazyElemMock1.Verify(e => e(), Times.Once);
            lazyElemMock2.Verify(e => e(), Times.Never);
            lazyCollectionMock.Verify(xs => xs(), Times.Never);

            //move to the 2nd element
            iter.MoveNext();

            //lazyElemMock2 should have been evaluated by now
            lazyElemMock1.Verify(e => e(), Times.Once);
            lazyElemMock2.Verify(e => e(), Times.Once);
            lazyCollectionMock.Verify(xs => xs(), Times.Never);

            //move to the 3rd element
            iter.MoveNext();

            //lazyCollectionMock should have been evaluated by now
            lazyElemMock1.Verify(e => e(), Times.Once);
            lazyElemMock2.Verify(e => e(), Times.Once);
            lazyCollectionMock.Verify(xs => xs(), Times.Once);
        }

        [Fact]
        public void ResultSequence_IgnoresFurtherClears()
        {
            var builder = Sequence.NewBuilder<int>()
                                  .Append(1, 2, 3);

            var sequence = builder.ToSequence();

            //clear
            builder.Clear();

            Assert.Equal(new[] {1, 2, 3}, sequence);
        }

        [Fact]
        public void ResultSequence_IgnoresFurtherAppends()
        {
            var builder = Sequence.NewBuilder<int>()
                                  .Append(1, 2, 3);

            var sequence = builder.ToSequence();

            //append more elements
            builder.Append(9);

            Assert.DoesNotContain(9, sequence);
        }

        [Fact]
        public void Clear_ClearsBuffer()
        {
            var builder = Sequence.NewBuilder<int>()
                                  .Append(1)
                                  .Clear();

            Assert.Empty(builder.ToSequence());
        }

        [Fact]
        public void AppendParams_ThrowsException_When_InputIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Sequence.NewBuilder<int>().Append(null as int[]));
        }

        [Fact]
        public void AppendEnumerable_ThrowsException_When_InputIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Sequence.NewBuilder<int>().Append(null as IEnumerable<int>));
        }
    }
}
