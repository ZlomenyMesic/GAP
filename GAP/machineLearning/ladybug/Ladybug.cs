//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

namespace GAP.machineLearning.ladybug;

internal static class Ladybug {
    private static readonly string RenameDataScriptPath = @"ladybug\RenameData.py";
    private static readonly string DataSubsetsScriptPath = @"ladybug\DataSubsets.py";
    private static readonly string TrainModelScriptPath = @"ladybug\TrainModel.py";
    private static readonly string ApplicationScriptPath = @"ladybug\Application.py";

    internal static void RenameData() {
        PythonWrapper.RunPythonScript(RenameDataScriptPath);
    }

    internal static void CreateDataSubsets() {
        PythonWrapper.RunPythonScript(DataSubsetsScriptPath);
    }

    internal static void TrainModel() {
        PythonWrapper.RunPythonScript(TrainModelScriptPath);
    }

    internal static void RunGenerator() {
        PythonWrapper.RunPythonScript(ApplicationScriptPath);
    }
}
