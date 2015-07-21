﻿using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiskBenchmark.Library
{
    /// <summary>
    /// 测试文件的有关操作
    /// </summary>
    public static class TestFile
    {
        #region 外部函数
        #endregion

        #region 标志位
        private const uint FILE_FLAG_NO_BUFFERING = 0x20000000;
        private const uint FILE_FLAG_WRITE_THROUGH = 0x80000000;
        private const uint FileFlags = (FILE_FLAG_NO_BUFFERING | FILE_FLAG_WRITE_THROUGH);
        #endregion

        private static readonly string workDirectory = @"\SSD测试工具临时文件夹";
        private static readonly string fileName = "临时文件.tmp";

        //private static Random random = new Random();

        /// <summary>
        /// 打开测试用文件流，供测试程序使用。
        /// </summary>
        /// <param name="partition">测试分区</param>
        /// <param name="testType">测试类型</param>
        /// <param name="bufferSize">一个大于零的正 System.Int32 值，表示缓冲区大小。 对于 1 和 8 之间的 bufferSize 值，缓冲区的实际大小设置为 8 字节。</param>
        /// <param name="work">测试程序</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:验证公共方法的参数", MessageId = "3")]
        public static void OpenFileStream(PartitionInfo partition, TestType testType, int bufferSize, Action<FileStream> work)
        {
            var fileAccess = (testType.HasFlag(TestType.Read) ? FileAccess.Read : new FileAccess()) | (testType.HasFlag(TestType.Write) ? FileAccess.Write : new FileAccess());
            var fullWorkDirectory = partition.DeviceId + workDirectory;
            var fileDirectory = fullWorkDirectory + fileName;

            DeleteDirectory(fullWorkDirectory);             // 如果有，删除目录
            Directory.CreateDirectory(fullWorkDirectory);   // 创建测试目录            
            DecompressFolder(fullWorkDirectory);            // 将测试目录的压缩选项取消
            //打开文件句柄
            using (SafeFileHandle handle = NativeMethods.CreateFile(fileDirectory, fileAccess, FileShare.None, IntPtr.Zero, FileMode.OpenOrCreate, FileFlags, IntPtr.Zero))
            {
                int errorcode = Marshal.GetLastWin32Error();
                if (handle.IsInvalid)
                {
                    //TODO: 本地化
                    throw new IOException(string.Format("测试临时文件创建失败。错误：{0}", errorcode), new Win32Exception(errorcode));
                }
                //打开文件流
                using (FileStream stream = new FileStream(handle, FileAccess.Read, bufferSize, false))
                {
                    work(stream);
                }
            }
            DeleteDirectory(fullWorkDirectory);
        }

        public static uint DecompressFolder(string path)
        {
            using (ManagementObject obj2 = new ManagementObject("Win32_Directory.Name=\"" + path.Replace(@"\", @"\\") + "\""))
            {
                return (uint)obj2.InvokeMethod("UnCompress", null, null).Properties["ReturnValue"].Value;
            }
        }

        private static void DeleteDirectory(string directory)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            if (directoryInfo.Exists)
            {
                directoryInfo.Delete(true);
            }
        }
    }
}
