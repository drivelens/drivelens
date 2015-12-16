namespace Drivelens.BenchLibrary

type BenchmarkType =
    | Read = 0x01
    | Write = 0x02

[<System.Flags>]
type BenchmarkFlags =
    | None = 0x0
    | Compressible = 0x04