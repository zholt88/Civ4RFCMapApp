using System;
using System.Collections.Generic;

namespace Civ4RFCMapApp.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IReadOnlyListExtensions
    {
        public static int FindIndex<T>(this IReadOnlyList<T> collection, Predicate<T> match)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                if (match.Invoke(collection[i])) return i;
            }
            return -1;
        }
    }
}
