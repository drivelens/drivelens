﻿module Drivelens.BenchLibrary.Algorithm

open System.Collections.Concurrent
open System.Diagnostics

let cacheData = ConcurrentDictionary<int, byte[]>()
let random = System.Random()

let rec getData size compressible =
    match compressible with
    | true ->  
        //如果要求可以压缩，获取缓存中数据。
        let hasValue, cache = cacheData.TryGetValue size
        if hasValue then cache else getData size false
    | false -> 
        //如果要求不可压缩,重新计算
        let data : byte[] = Array.init size (fun _ -> 0uy)
        random.NextBytes data

        //更新缓存
        let f = new System.Func<int, byte[], byte[]>(fun a b -> b)
        cacheData.AddOrUpdate(size, data, f) |> ignore 

        data
        
let getTime work =
    //并没有必要调用GC
    //GC.Collect();
    let stopwatch = Stopwatch()

    stopwatch.Start()
    work ()
    stopwatch.Stop()

    stopwatch.Elapsed

let SequenceBenchmark blockSize blockCount stream work (flags : BenchmarkFlags) =
    let buffer = getData blockSize <| flags.HasFlag(BenchmarkFlags.Compressible)

    let constf x o = x
    {1 .. blockCount}
    |> Seq.map (constf <| getTime (constf <| work (buffer, 0, buffer.Length)))
    |> Seq.fold (+) (System.TimeSpan 0L)
