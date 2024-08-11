using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Net.Cougar.Utils
{
    public static class DistinctByKey
    {
        public static Predicate<T> DistinctByKeyFunc<T>(Func<T, object> keyExtractor)
        {
            var seen = new ConcurrentDictionary<object, byte>();
            return t => seen.TryAdd(keyExtractor(t), 0);
        }
    }
}