using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolBytes.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static void IsNotNull(this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
        }
    }
}
