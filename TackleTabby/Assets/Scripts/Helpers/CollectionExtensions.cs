using System.Collections.Generic;
using System.Linq;

public static class CollectionExtensions
{
    public static T[] ToSmartArray<T>(this IEnumerable<T> collection)
    {
        if (collection is T[] value)
            return value;

        return collection.ToArray();
    }
}