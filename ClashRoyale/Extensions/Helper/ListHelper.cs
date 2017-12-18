namespace ClashRoyale.Extensions.Helper
{
    using System;
    using System.Collections.Generic;

    internal static class ListHelper
    {
        internal static bool TryGet<T>(this List<T> List, Predicate<T> Match, out T Item)
        {
            Item = List.Find(Match);
            return Item != null;
        }

        internal static bool TryGetIndex<T>(this List<T> List, Predicate<T> Match, out int Index)
        {
            return (Index = List.FindIndex(Match)) != -1;
        }
    }
}