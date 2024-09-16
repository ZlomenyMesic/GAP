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
    internal static void RunPythonScript(string name, string args = "")
        => ExecuteCMD($"python {SCRIPT_DIR}{name} {args}");

    // cmd.exe arguments have to start with /c
    private static void ExecuteCMD(string cmd) {
        Process.Start(new ProcessStartInfo() {
            FileName = "cmd.exe",
            Arguments = $"/c {cmd}"
        });
    }
}
