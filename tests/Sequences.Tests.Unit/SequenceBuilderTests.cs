using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sequences.Tests
{
    public class SequenceBuilderTests
    {
        [Fact]
        public void ResultSequence_Includes()
        {
            var collection1 = new List<int> {1, 2, 3};
            var collection2 = Sequence.Range(4, 7);

            var builder = Sequence.NewBuilder<int>()
                                  .Append(collection1)
                                  .Append(collection2);

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
        public void ResultSequence_LazilyEvaluatesAppendedSequences()
        {
            var inputSequence = Sequence.Range(4, 7);

            var builder = Sequence.NewBuilder<int>()
                                  .Append(inputSequence);

            var resultSequence = builder.ToSequence();

            //assert that the input sequence hasn't been fully realized yet
            Assert.False(inputSequence.IsTailDefined);
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
        public void AppendEnumerable_ThrowsException_When_InputIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Sequence.NewBuilder<int>().Append(null));
        }

        [Fact]
        public void AppendParams_ThrowsException_When_InputIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Sequence.NewBuilder<int>().Append(null as IEnumerable<int>));
        }
    }
}
