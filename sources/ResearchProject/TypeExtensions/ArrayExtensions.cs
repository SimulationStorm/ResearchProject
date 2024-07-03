using System;
using System.Collections.Generic;
using System.Linq;

public static class ArrayExtensions
{
    public static T[][] SplitToVerticalChunks<T>(this T[,] matrix, int chunkCount)
    {
        ArgumentNullException.ThrowIfNull(matrix, nameof(matrix));

        int width = matrix.GetLength(0),
            height = matrix.GetLength(1);

        if (chunkCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(chunkCount), "should be greater than zero");

        if (chunkCount > width)
            chunkCount = width;

        int chunkWidth = width / chunkCount,
            remainingWidth = width % chunkCount,
            chunkSize = chunkWidth * height;

        IList<IList<T>> chunks = new List<IList<T>>(chunkCount);

        for (var i = 0; i < chunkCount; i++)
        {
            var chunk = new List<T>(chunkSize);

            int startX = i * chunkWidth,
                endX = startX + chunkWidth;

            for (var y = 0; y < height; y++)
                for (var x = startX; x < endX; x++)
                    chunk.Add(matrix[x, y]);

            chunks.Add(chunk);
        }

        for (var i = 0; i < remainingWidth; i++)
        {
            var chunk = chunks[i];

            var x = width - i - 1;
            for (var y = 0; y < height; y++)
                chunk.Add(matrix[x, y]);
        }

        return chunks.Select(Enumerable.ToArray).ToArray();
    }

    public static T RandomElement<T>(this T[] array) => array.RandomElementUsing(new Random());

    public static T RandomElementUsing<T>(this T[] array, Random rng) => array[rng.Next(array.Length)];

    public static void Shuffle<T>(this T[] array) => ShuffleUsing(array, new Random());

    public static void ShuffleUsing<T>(this T[] array, Random rng)
    {
        var n = array.Length;
        while (n > 1)
        {
            var k = rng.Next(n--);
            (array[k], array[n]) = (array[n], array[k]);
        }
    }
}