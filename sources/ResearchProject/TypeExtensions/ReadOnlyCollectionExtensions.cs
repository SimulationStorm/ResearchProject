using System;
using System.Collections.Generic;
using System.Linq;

public static class ReadOnlyCollectionExtensions
{
    public static T RandomElement<T>(this IReadOnlyCollection<T> enumerable) => enumerable.RandomElementUsing(new Random());

    public static T RandomElementUsing<T>(this IReadOnlyCollection<T> collection, Random rng)
    {
        if (collection is T[] array)
            return array[rng.Next(collection.Count)];

        return collection.ElementAt(rng.Next(collection.Count));
    }
}