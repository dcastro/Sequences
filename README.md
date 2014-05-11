# Sequences

Sequences is a port of Scala's [`Stream[+A]`][3] to C#.

A `Sequence<T>` is an immutable lazy list whose elements are only evaluated when they are needed. A sequence is composed by a *head* (the first element) and a lazily-evaluated *tail* (the remaining elements).

The fact that the tail is lazily-evaluated, makes it easy to represent infinite series or sets. For example, here's how to represent the set of all natural numbers.


```cs
public ISequence<int> Naturals(int start)
{
    return new Sequence<int>( head: start,
                              tail: () => Naturals(start + 1));
}

var naturals = Naturals(1);

//take the first 5 natural numbers
naturals.Take(5).ForEach(Console.Write); //prints 12345
```

Or, even simpler:

```cs
var naturals = Sequence.From(1);
```

Sequences also features memoization, i.e., the sequence stores previously computed values to avoid re-evaluation.

```chsarp

//start with number 1, and then keep adding 2 to the previous number
var odds = Sequence.Iterate(1, odd =>
    {
        Console.WriteLine("Adding " + odd + " + 2");
        return odd + 2;
    });

odds.Take(3).ForEach(Console.WriteLine);
odds.Take(5).ForEach(Console.WriteLine);

//prints
//1
//Adding 1 + 2
//3
//Adding 3 + 2
//5

//and then
//1
//3
//5
//Adding 5 + 2
//7
//Adding 7 + 2
//9
```

You can iterate through an infinite sequence for as long as you want. As long as you don't hold onto its head, each sequence will be elected for garbage collection as soon as you move to the next value. This prevents an infinite sequence from occupying a large and growing ammount of memory.

```cs
foreach (var odd in Sequence.Iterate(1, odd => odd + 2))
{
    //when you move to Sequence(11, ?),
    //the previous Sequence(9, ?) is elected for collection.
}
```

## Examples

The above natural numbers example is very simple. But Sequences allow for so much more. So let's explore some more complex examples.

#### Fibonacci sequence

The Fibonacci sequence is a famous series in mathematics, where each fibonacci number is defined as the sum of the two previous fibonacci numbers, i.e. `F(n) = F(n-1) + F(n-2)`, with seed values `F(0) = 0` and `F(1) = 1`.

In scala, the fibonacci sequence is commonly expressed as follows:

```scala
val fibs: Stream[Int] = 0 #:: 1 #:: fibs.zip(fibs.tail).map { n => n._1 + n._2 }
```

In C#, the syntax is a little more verbose, but still readable:

```cs
Func<Tuple<int, int>, int> sum = pair => pair.Item1 + pair.Item2;

ISequence<int> fibs = null;

fibs = Sequence.With(0, 1)               //start with (0, 1, ?)
               .Concat(() =>             //and then
                   fibs.Zip(fibs.Tail)   //zip the sequence with its tail (i.e., (0,1), (1,1), (1,2), (2,3), (3, 5))
                       .Select(sum));    //select the sum of each pair (i.e., 1, 2, 3, 5, 8)
```

The following implementation shows a more efficient way of representing the fibonacci sequence:

```cs
using System.Numerics;

//current and next are any two consecutive fibonacci numbers.
ISequence<BigInteger> Fibs(BigInteger current, BigInteger next)
{
    return new Sequence<BigInteger>(current, () => Fibs(next, current + next));
}

var fibs = Fibs(0, 1);

fibs.Take(10).ForEach(Console.WriteLine);

//prints 0 1 1 2 3 5 8 13 21 34
```

#### Prime numbers

One way to find every prime number in a given range is to use the [Sieve of Eratosthenes][4].
To find the prime numbers up to 100, the sieve goes like this:

1. Start with a list representing the range [2, 100].
2. Let *p* equal 2, the first prime number.
3. Remove every multiple of *p* from the list, except *p* itself.
4. Find the number greater than *p* that hasn't been removed from the list.
  a) If there is no such number, stop;
  b) Otherwise, let *p* now be this number, and repeat from step 3.

Here's a non-optimized way of implementing the sieve as a sequence.

```cs

var range = Sequence.Range(2, 101);
var primes = PrimesWithin(range);

Console.WriteLine(primes.Take(5).MkString(" "));

//prints: 2 3 5 7 11

public ISequence<int> PrimesWithin(ISequence<int> range)
{
    if (range.IsEmpty)
        return Sequence.Empty<int>();

    //take the next prime number
    var p = range.Head;

    //skip p, and remove further multiples of p
    var filtered = range.Tail.Where(num => num % p != 0);

    return new Sequence<int>(p, () => PrimesWithin(filtered));
}
```



For more examples, refer to the [functional tests project][1].


## Documentation
Documentation is available at [dcastro.github.io/Sequences][2].


## Limitations

Due to a limitation in [generic type constraints][5] as of C# 5.0, sequences are *not* covariant, as opposed to Scala's `Stream[+A]`.

Take the signature of `copyToBuffer` in Scala as an example:

```scala
copyToBuffer[B >: A](dest: Buffer[B]): Unit
```

The constraint `B >: A` (read *A derives from B*) cannot be expressed in C# (even though the opposite can be expressed as `where B : A` or *B derives from A*) unless `A` is also one of *copyToBuffer*'s type parameters - which it isn't.


[1]: https://github.com/dcastro/Sequences/tree/master/tests/Sequences.Tests.Functional
[2]: http://diogocastro.com/Sequences
[3]: http://www.scala-lang.org/api/current/index.html#scala.collection.immutable.Stream
[4]: http://en.wikipedia.org/wiki/Sieve_of_Eratosthenes
[5]: http://msdn.microsoft.com/en-gb/library/d5x73970.aspx

