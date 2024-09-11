//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

using System.Diagnostics;

namespace GAP;

internal static class PythonWrapper {
    private static readonly string PY_EXE_PATH = $@"C:\Users\{Environment.UserName}\AppData\Local\Programs\Python\Python312\python.exe";
    private static readonly string SCRIPT_PATH = $@"..\..\..\machinelearning\";

    // uses the standard path for Python 3.12
    // script must be located in directory machinelearning
    internal static string RunScript(string name, string args = "") {
        ProcessStartInfo start = new() {
            FileName = PY_EXE_PATH,
            Arguments = $"{SCRIPT_PATH}{name} {args}",
            UseShellExecute = false,
            RedirectStandardOutput = true
        };

        using Process? process = Process.Start(start);
        if (process == null) {
            Console.WriteLine($"script {name} failed");
            return string.Empty;
        }

        using StreamReader reader = process.StandardOutput;
        return reader.ReadToEnd();
    }
}
