module Drivelens.BenchLibrary.NativeMethods

open Microsoft.Win32.SafeHandles
open System
open System.IO
open System.Runtime.InteropServices

[<DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)>]
extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, FileShare dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, int32 dwFlagsAndAttributes, IntPtr hTemplateFile)