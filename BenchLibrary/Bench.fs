[<AutoOpen>]
module private Drivelens.BenchLibrary.BenchModule

open Drivelens.DetectionLibrary
open System.Threading
open System.IO

let GetReadOrWriteAction (stream : Stream) =
    function
    | BenchmarkType.Read ->
        fun (bytes, i, length) -> ignore (stream.Read(bytes, i, length))
    | BenchmarkType.Write -> stream.Write
    | _ -> raise <| System.ArgumentException(" benchType 参数不能同时具有Write和Read标志。", "benchType")

let blockBench (partition : PartitionInfo) benchType flags blocksize blockCount algorithm =
    let result = openFileStream partition benchType blocksize (
        fun stream ->
            let work = GetReadOrWriteAction stream benchType
            algorithm stream work flags
    )
    IOSpeed( result, blockCount, int64 (blockCount * blocksize))