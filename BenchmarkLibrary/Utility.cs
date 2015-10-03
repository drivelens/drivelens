using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drivelens.BenchmarkLibrary
{
    public static class Utility
    {
        /// <summary>
        /// 遍历序列中的元素，执行算法。
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="work">要执行的代码</param>
        /// <returns>原序列</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> work)
        {
            foreach (var item in list)
            {
                work(item);
            }
            return list;
        }

        /// <summary>
        /// 测量函数运行时间。
        /// </summary>
        /// <param name="work">待测函数</param>
        /// <returns></returns>
        public static TimeSpan GetTime(Action work)
        {
            //并没有必要调用GC
            //GC.Collect();
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            work();
            stopwatch.Stop();

            return stopwatch.Elapsed;
        }

        //private static Dictionary<int, byte[]> cacheData = new Dictionary<int, byte[]>();
        private static ConcurrentDictionary<int, byte[]> cacheData = new ConcurrentDictionary<int, byte[]>();

        private static Random random = new Random();

        /// <summary>
        /// 以线程安全的方式计算随机数据。
        /// </summary>
        /// <param name="size">需要的字节数</param>
        /// <param name="compressible">是否要求可压缩</param>
        /// <returns></returns>
        public static byte[] GetData(int size, bool compressible)
        {
            if (compressible)
            {
                //如果要求可以压缩，获取缓存中数据。
                byte[] cache;
                bool hasValue = cacheData.TryGetValue(size, out cache);
                return hasValue ? cache : GetData(size, false);
            }
            else
            {
                //如果要求不可压缩,重新计算
                var data = new byte[size];
                random.NextBytes(data);

                //更新缓存
                cacheData.AddOrUpdate(size, data, (key, val) => val);

                return data;
            }
        }
    }
}
