using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolBytes.Core.Utils
{
    public static class When
    {
        public static void NotNull<T>(T obj, Action then)
        {
            if (obj != null) then?.Invoke();
        }

        public static async Task NotNull<T>(T obj, Func<Task> then)
        {
            if (obj != null && then != null) await then.Invoke();
        }
    }
}
