using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtensions
{
    // Todo: it seems to that all method can be implemented for enumerable base class only (with checkings: if is array/collection/list etc.)
    public static bool AreAllItemsUnique<T>(this IEnumerable<T> enumerable) => enumerable.Distinct().Count() == enumerable.Count();

    public static bool IsEmpty<T>(this IEnumerable<T> enumerable) => !enumerable.Any();

    public static T RandomElementThreadSafe<T>(this IEnumerable<T> enumerable) => enumerable.RandomElementUsing(Random.Shared);

    public static T RandomElement<T>(this IEnumerable<T> enumerable) => enumerable.RandomElementUsing(new Random());

    public static T RandomElementUsing<T>(this IEnumerable<T> enumerable, Random rng)
    {
        var array = enumerable as T[] ?? enumerable.ToArray();
        return array[rng.Next(array.Length)];
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) => source.ShuffleUsing(new Random());

    public static IEnumerable<T> ShuffleUsing<T>(this IEnumerable<T> source, Random rng) => source.ShuffleIterator(rng);

    private static IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> source, Random rng)
    {
        var buffer = source.ToList();
        for (var i = 0; i < buffer.Count; i++)
        {
            var j = rng.Next(i, buffer.Count);
            yield return buffer[j];

            buffer[j] = buffer[i];
        }
    }
}