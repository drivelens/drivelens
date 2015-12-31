using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drivelens.DetectionLibrary
{
    /// <summary>
    /// 可用单个标识符来确定的对象。
    /// </summary>
    /// <typeparam name="T">用于比较的标识符类型。</typeparam>
    public interface IIdentifiable<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 获取标识符。
        /// </summary>
        T Identifier { get; }
    }
}
