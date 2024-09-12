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
         => RunProcess(GetPythonExecutablePath(), $"{SCRIPT_DIR}{name} {args}", false);

    private static string GetPythonExecutablePath()
        => RunProcess("cmd.exe", "python -c \"import sys; print(sys.executable)\"", true);

    // cmd.exe arguments have to start with /c
    private static string RunProcess(string name, string args, bool cmd) {
        ProcessStartInfo start = new() {
            FileName = name,
            Arguments = $"{(cmd ? "/c " : "")}{args}",
            UseShellExecute = false,
            RedirectStandardOutput = true
        };

        using Process? process = Process.Start(start);
        if (process == null) {
            Console.WriteLine($"running {name} failed");
            return string.Empty;
        }

        return process.StandardOutput.ReadToEnd() ?? "";
    }
}
