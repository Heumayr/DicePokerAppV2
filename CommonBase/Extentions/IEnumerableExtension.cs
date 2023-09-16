using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonBase.Extentions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> sequence)
            where T : class
        {
            return sequence.Where(e => e != null).Select(e => e!);
        }

    }
}
