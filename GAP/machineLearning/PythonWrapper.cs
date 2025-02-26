//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 
//      founded 11.9.2024
//

using System.Diagnostics;

namespace GAP.machineLearning;

internal static class PythonWrapper {
    internal static string SCRIPT_DIR = $@"..\..\..\machineLearning\";

    // when Python is installed via Microsoft Store, the command "python" doesn't
    // exist, so "py" has to be run instead
    internal static string PYTHON_CMD = "python";

    /// <summary>
    /// executes a python script.
    /// the script MUST be located in machineLearning directory!
    /// </summary>
    /// <param name="name">script/file name</param>
    /// <param name="args">optional arguments</param>
    internal static void RunPythonScript(string name, string args = "") {
        try {
            ExecuteCMD($"{PYTHON_CMD} {SCRIPT_DIR}{name} {args}");
        } catch {
            Console.WriteLine($"Could not run script: {name}");
        }
    }

    /// <summary>
    /// executes a command using cmd.exe.
    /// </summary>
    /// <param name="cmd">command to be executed</param>
    // ReSharper disable once InconsistentNaming
    private static void ExecuteCMD(string cmd) {
        Process.Start(new ProcessStartInfo() {
            FileName = "cmd.exe",
            Arguments = $"/c {cmd}"
        });
    }
}
