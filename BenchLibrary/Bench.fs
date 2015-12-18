[<AutoOpen>]
module private Drivelens.BenchLibrary.BenchModule

open Drivelens.DetectionLibrary
open System.Threading
open System.IO

let getTestResult (partition : PartitionInfo) benchType flags blocksize (cancellationToken : CancellationToken) algorithm =
    openFileStream partition benchType blocksize (
        fun stream ->
            let work = GetReadOrWriteAction(benchType, stream)
            algorithm(stream, work, flags, cancellationToken)
    )
    IOSpeed( result, BlockCount, BlockCount * blocksize)
