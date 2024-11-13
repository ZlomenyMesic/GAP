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
        WaitForOutput();
    }

    internal static void RunGenerator() {
        PythonWrapper.RunPythonScript(ApplicationScriptPath);
        WaitForOutput();
    }
    private static void WaitForOutput() {
        while (!File.Exists("DONE"))
            Thread.Sleep(500);
        File.Delete("DONE");
    }
}
