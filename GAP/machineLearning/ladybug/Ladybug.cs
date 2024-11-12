//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

namespace GAP.machineLearning.ladybug;

internal static class Ladybug {
    private static readonly string TrainingScriptPath = @"ladybug\ModelTraining.py";
    private static readonly string ApplicationScriptPath = @"ladybug\ModelApplication.py";

    internal static void TrainModel() {
        PythonWrapper.RunPythonScript(TrainingScriptPath);
    }

    internal static void RunGenerator() {
        PythonWrapper.RunPythonScript(ApplicationScriptPath);
    }
}
