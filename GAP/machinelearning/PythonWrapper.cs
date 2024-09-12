//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

using System.Diagnostics;

namespace GAP;

internal static class PythonWrapper {
    private static readonly string SCRIPT_DIR = $@"..\..\..\machinelearning\";

    // script must be located in directory machinelearning
    internal static string RunPythonScript(string name, string args = "")
        => ExecuteCMD($"python {SCRIPT_DIR}{name} {args}");

    // cmd.exe arguments have to start with /c
    private static string ExecuteCMD(string cmd) {
        ProcessStartInfo start = new() {
            FileName = "cmd.exe",
            Arguments = $"/c {cmd}",
            UseShellExecute = false,
            RedirectStandardOutput = true
        };

        using Process? process = Process.Start(start);
        if (process == null) {
            Console.WriteLine($"running cmd.exe failed");
            return string.Empty;
        }

        return process.StandardOutput.ReadToEnd() ?? "";
    }
}
