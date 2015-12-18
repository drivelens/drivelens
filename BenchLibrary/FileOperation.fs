[<AutoOpen>]
module Drivelens.BenchLibrary.BenchmarkFile

open Drivelens.DetectionLibrary
open Microsoft.Win32.SafeHandles
open System.ComponentModel
open System.IO
open System.Management
open System.Runtime.InteropServices


let private FILE_FLAG_NO_BUFFERING = 0x20000000u
let private FILE_FLAG_WRITE_THROUGH = 0x80000000u
let private FileFlags = FILE_FLAG_NO_BUFFERING ||| FILE_FLAG_WRITE_THROUGH

let private workDirectory = @"\SSD测试工具临时文件夹"
let private fileName = "临时文件.tmp"


let DeleteDirectory directory =
    let directoryInfo = DirectoryInfo directory
    if directoryInfo.Exists then directoryInfo.Delete true

let DecompressFolder (path : string) =
    let path' =
        path.Replace(@"\", @"\\")
        |> sprintf "Win32_Directory.Name=\"%s\""
    use obj  = new ManagementObject(path')
    downcast obj.InvokeMethod("Uncompress", null, null).Properties.["ReturnValue"].Value
    

let OpenFileHandle (partition : PartitionInfo) (benchmarkType : BenchmarkType) work =
    let fileAccess = 
        if benchmarkType.HasFlag(BenchmarkType.Read) then FileAccess.Read else FileAccess() 
        ||| if benchmarkType.HasFlag(BenchmarkType.Write) then FileAccess.Write else FileAccess()
    let fullWorkDirectory = partition.DeviceId + workDirectory
    let fileDirectory = sprintf @"%s\%s" fullWorkDirectory fileName

    DeleteDirectory fullWorkDirectory                       // 如果有，删除目录
    Directory.CreateDirectory fullWorkDirectory |> ignore   // 创建测试目录            
    DecompressFolder fullWorkDirectory                      // 将测试目录的压缩选项取消
    //打开文件句柄
    use handle = NativeMethods.CreateFile(fileDirectory, fileAccess, FileShare.None, System.IntPtr.Zero, FileMode.OpenOrCreate, int32 FileFlags, System.IntPtr.Zero)
    let errorcode = Marshal.GetLastWin32Error()
    if handle.IsInvalid then
        //TODO: 本地化
        raise <| IOException(sprintf "测试临时文件创建失败。错误：%d" errorcode, Win32Exception(errorcode));
    work(handle)
    DeleteDirectory(fullWorkDirectory)


let openFileStream (partition : PartitionInfo) benchType bufferSize benchmarkType work =
    OpenFileHandle partition benchmarkType (
        fun handle ->
            //打开文件流
            use stream = new FileStream(handle, FileAccess.Read, bufferSize, false)
            work stream
    )
