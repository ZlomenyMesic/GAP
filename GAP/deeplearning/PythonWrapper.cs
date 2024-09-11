using System.Diagnostics;
//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

namespace GAP;

internal static class PythonWrapper {
    private static string RunScript(string path, string args) {
        ProcessStartInfo start = new() {
            FileName = "my/full/path/to/python.exe",
            Arguments = string.Format($"{path} {args}"),
            UseShellExecute = false,
            RedirectStandardOutput = true
        };

        using Process? process = Process.Start(start);
        if (process == null) {
            Console.WriteLine($"failed running script {path}");
            return string.Empty;
        }

        using StreamReader reader = process.StandardOutput;
        return reader.ReadToEnd();
    }
}
