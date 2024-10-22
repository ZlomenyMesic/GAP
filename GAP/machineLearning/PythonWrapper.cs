//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

using System.Diagnostics;

namespace GAP.machinelearning;

internal static class PythonWrapper {
    private static readonly string SCRIPT_DIR = $@"..\..\..\machineLearning\";

    // script must be located in directory machineLearning
    /// <summary>
    /// executes a python script.
    /// the script MUST be located in machineLearning directory!
    /// </summary>
    /// <param name="name">script/file name</param>
    /// <param name="args">optional arguments</param>
    internal static void RunPythonScript(string name, string args = "")
        => ExecuteCMD($"python {SCRIPT_DIR}{name} {args}");

    /// <summary>
    /// executes a command using cmd.exe.
    /// </summary>
    /// <param name="cmd">command to be executed</param>
    private static void ExecuteCMD(string cmd) {
        Process.Start(new ProcessStartInfo() {
            FileName = "cmd.exe",
            Arguments = $"/c {cmd}"
        });
    }
}
