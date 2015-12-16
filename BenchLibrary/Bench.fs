﻿[<AutoOpen>]
module private Drivelens.BenchLibrary.BenchModule

open Drivelens.DetectionLibrary
open System.Threading
open System.IO

let getTestResult (partition : PartitionInfo) benchType flags Blocksize (cancellationToken : CancellationToken) algorithm =
    openFileStream partition benchType BlockSize (
        fun stream ->
            let work = GetReadOrWriteAction(benchType, stream)
            algorithm(stream, work, flags, cancellationToken)
    )
    IOSpeed(time: result, ioCount: BlockCount, bytes: BlockCount * BlockSize)
