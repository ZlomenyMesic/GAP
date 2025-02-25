//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 
//      founded 11.9.2024
//

namespace GAP.machineLearning.ladybug.scripts;

// while the scripts are accessible through C#, it is better to use them directly
internal static class Scripts {
    private static readonly string RenameDataPath = @"scripts\RenameData.py";
    private static readonly string CreateSubsetsPath = @"scripts\CreateSubsets.py";
    private static readonly string ResizeImagesPath = @"scripts\ResizeImages.py";
    private static readonly string PrintModelSummaryPath = @"scripts\PrintModelSummary.py";

    internal static void RenameData() {
        PythonWrapper.RunPythonScript(RenameDataPath);
    }

    internal static void CreateDataSubsets() {
        PythonWrapper.RunPythonScript(CreateSubsetsPath);
    }

    internal static void ResizeImages() {
        PythonWrapper.RunPythonScript(ResizeImagesPath);
    }

    internal static void PrintModelSummary() {
        PythonWrapper.RunPythonScript(PrintModelSummaryPath);
    }
}
