//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 
//      founded 11.9.2024
//

namespace GAP.machineLearning.scripts;

// while the scripts are accessible through C#, it is better to use them directly
internal static class Scripts {
    private const string RENAME_DATA_PATH = @"scripts\RenameData.py";
    private const string CREATE_SUBSETS_PATH = @"scripts\CreateSubsets.py";
    private const string RESIZE_IMAGES_PATH = @"scripts\ResizeImages.py";
    private const string PRINT_MODEL_SUMMARY_PATH = @"scripts\PrintModelSummary.py";

    internal static void RenameData() {
        PythonWrapper.RunPythonScript(RENAME_DATA_PATH);
    }

    internal static void CreateDataSubsets() {
        PythonWrapper.RunPythonScript(CREATE_SUBSETS_PATH);
    }

    internal static void ResizeImages() {
        PythonWrapper.RunPythonScript(RESIZE_IMAGES_PATH);
    }

    internal static void PrintModelSummary() {
        PythonWrapper.RunPythonScript(PRINT_MODEL_SUMMARY_PATH);
    }
}
