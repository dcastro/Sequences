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

        [Fact]
        public void Enumerator_AllowsGCToCollectSequences()
        {
            var seqReference = new WeakReference<ISequence<int>>(
                Sequence.From(1));

            ISequence<int> temp;
            Assert.True(seqReference.TryGetTarget(out temp));

            IEnumerator<int> iter = temp.GetEnumerator();
            temp = null;

            iter.MoveNext(); //move to the original sequence    - Sequence(1, ?)
            iter.MoveNext(); //move to the second sequence      - Sequence(2, ?)

            //collect
            GC.Collect();

            //the iterator should no longer hold a reference to the original Sequence(1, ?) 
            //and, so, the GC should have been able to clear the sequence the WeakReference points to.
            Assert.False(seqReference.TryGetTarget(out temp));

            /**
             * Keep the iterator alive.
             * If the iterator was collected, the above assertion could be a "false positive" - the weak reference could have been collected
             * not because the iterator allows it, but because the iterator was *also* collected.
             */
            GC.KeepAlive(iter);
        }

        [Fact]
        public void SlidingEnumerator_AllowsGCToCollectSequences()
        {
            var seqReference = new WeakReference<ISequence<int>>(
                Sequence.From(1));

            ISequence<int> temp;
            Assert.True(seqReference.TryGetTarget(out temp));

            IEnumerator<ISequence<int>> iter = temp.Sliding(2).GetEnumerator();
            temp = null;

            iter.MoveNext(); //move to the first sliding window     - Sequence(1, 2)
            iter.MoveNext(); //move to the second sliding window    - Sequence(2, 3)

            //collect
            GC.Collect();

            //the iterator should no longer hold a reference to the original Sequence(1, ?) 
            //and, so, the GC should have been able to clear the sequence the WeakReference points to.
            Assert.False(seqReference.TryGetTarget(out temp));

            /**
             * Keep the iterator alive.
             * If the iterator was collected, the above assertion could be a "false positive" - the weak reference could have been collected
             * not because the iterator allows it, but because the iterator was *also* collected.
             */
            GC.KeepAlive(iter);
        }
    }
}
