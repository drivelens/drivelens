namespace Drivelens.BenchLibrary

type BenchmarkType =
    | Read = 0x01
    | Write = 0x02
    | ReadWrite = 0x03

[<System.Flags>]
type BenchmarkFlags =
    | None = 0x0
    | Compressible = 0x04

[<Struct>]
type IOSpeed(time : System.TimeSpan, ioCount : int, bytes : int64) =
    member __.Time = time
    member __.IoCount = ioCount
    member __.Bytes = bytes
    member __.Megabytes = bytes / 0x100000L
    member this.MegabytePerSecond = double this.Megabytes / this.Time.TotalSeconds
    member this.IOPerSecond = double this.IoCount / this.Time.TotalSeconds