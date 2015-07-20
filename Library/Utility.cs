using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskBenchmark.Library
{
    static class Utility
    {
        public static IEnumerable<T> ForEach<T, TResult>(this IEnumerable<T> list, Func<T, TResult> work)
        {
            foreach (var item in list)
            {
                work(item);
            }
            return list;
        }
    }
}
