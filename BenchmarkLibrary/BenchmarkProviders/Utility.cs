using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drivelens.BenchmarkLibrary.BenchmarkProviders
{
    static class Utility
    {
        public static Action<byte[], int, int> GetReadOrWriteAction(BenchmarkType type, Stream stream)
        {
            if (type.HasFlag(BenchmarkType.Read) && !type.HasFlag(BenchmarkType.Write))
                return ((bytes, i, length) => stream.Read(bytes, i, length));
            else if (type.HasFlag(BenchmarkType.Write) && !type.HasFlag(BenchmarkType.Read))
                return stream.Write;
            else
                throw new ArgumentException($"{nameof(type)}参数不能同时具有Write和Read标志。", nameof(type));
        }
    }
}
