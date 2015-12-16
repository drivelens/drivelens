[<AutoOpen>]
module Drivelens.BenchLibrary.BenchmarkFile

open Drivelens.DetectionLibrary
open System.IO


let private FILE_FLAG_NO_BUFFERING = 0x20000000u
let private FILE_FLAG_WRITE_THROUGH = 0x80000000u
let private FileFlags = FILE_FLAG_NO_BUFFERING ||| FILE_FLAG_WRITE_THROUGH



let OpenFileHandle partition benchmarkType work =
    let fileAccess = 
        if benchmarkType.HasFlag(BenchmarkType.Read) then FileAccess.Read else FileAccess() 
        ||| if benchmarkType.HasFlag(BenchmarkType.Write) then FileAccess.Write else FileAccess()
    let fullWorkDirectory = partition.DeviceId + workDirectory;
    let fileDirectory = $@"{fullWorkDirectory}\{fileName}";

    DeleteDirectory(fullWorkDirectory);             // 如果有，删除目录
    Directory.CreateDirectory(fullWorkDirectory);   // 创建测试目录            
    DecompressFolder(fullWorkDirectory);            // 将测试目录的压缩选项取消
    //打开文件句柄
    use SafeFileHandle handle = NativeMethods.CreateFile(fileDirectory, fileAccess, FileShare.None, IntPtr.Zero, FileMode.OpenOrCreate, FileFlags, IntPtr.Zero)
    let errorcode = Marshal.GetLastWin32Error();
    if (handle.IsInvalid)
                    //TODO: 本地化
                    throw new IOException($"测试临时文件创建失败。错误：{errorcode}", new Win32Exception(errorcode));
    work(handle)
    DeleteDirectory(fullWorkDirectory);


let openFileStream (partition : PartitionInfo) benchType bufferSize work benchmarkType=
    OpenFileHandle partition benchmarkType (
        fun handle ->
            //打开文件流
            use stream = new FileStream(handle, FileAccess.Read, bufferSize, false)
            work stream
    )
