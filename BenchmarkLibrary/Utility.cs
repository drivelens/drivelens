using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskMagic.BenchmarkLibrary
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
            GC.Collect();
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            work();
            stopwatch.Stop();

            return stopwatch.Elapsed;
        }

        private static Dictionary<int, byte[]> cacheData = new Dictionary<int, byte[]>();

        private static Random random = new Random();

        /// <summary>
        /// 计算随机数据。
        /// </summary>
        /// <param name="size">需要的字节数</param>
        /// <param name="compressible">是否可压缩</param>
        /// <returns></returns>
        public static byte[] GetData(int size, bool compressible)
        {
            if (compressible)
            {
                byte[] cache;
                cacheData.TryGetValue(size, out cache);
                return cache ?? GetData(size, false);
            }
            else
            {
                var data = new byte[size];
                random.NextBytes(data);

                if (!cacheData.ContainsKey(size))
                    cacheData.Add(size, data);

                return data;
            }
        }

        public static Action<byte[], int, int> GetReadOrWriteAction(BenchmarkType type, Stream stream)
        {
            if (type.HasFlag(BenchmarkType.Read) && !type.HasFlag(BenchmarkType.Write))
                return ((bytes, i, length) => stream.Read(bytes, i, length));
            else if (type.HasFlag(BenchmarkType.Write) && !type.HasFlag(BenchmarkType.Read))
                return stream.Write;
            else
                throw new ArgumentException($"{nameof(type)}参数不能同时具有Write和Read标志。",nameof(type));
        }
    }
}
