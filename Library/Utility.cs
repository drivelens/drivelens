using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskBenchmark.Library
{
    public static class Utility
    {
        public static IEnumerable<T> ForEach<T, TResult>(this IEnumerable<T> list, Func<T, TResult> work)
        {
            foreach (var item in list)
            {
                work(item);
            }
            return list;
        }

        public static TimeSpan GetTime(Action work)
        {
            GC.Collect();
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            work();
            stopwatch.Stop();

            return stopwatch.Elapsed;
        }

        private static byte[] cacheData;

        private static Random random = new Random();

        public static byte[] GetData(int size, bool compressible)
        {
            if (compressible)
            {
                return cacheData ?? GetData(size, false);
            }
            else
            {
                var data = new byte[size];
                random.NextBytes(data);
                cacheData = data;
                return data;
            }
        }
    }
}
