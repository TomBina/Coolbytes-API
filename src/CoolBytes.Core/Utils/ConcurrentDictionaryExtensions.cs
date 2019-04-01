using System.Collections.Concurrent;

namespace CoolBytes.Core.Utils
{
    public static class ConcurrentDictionaryExtensions
    {
        public static TValue Get<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key) 
            => dict.TryGetValue(key, out var value) ? value : default;
    }
}