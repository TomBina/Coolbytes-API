using System;

namespace CoolBytes.Core.Extensions
{
    public static class StringExtensions
    {
        public static void IsNotNullOrWhiteSpace(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(str));
        }
    }
}
