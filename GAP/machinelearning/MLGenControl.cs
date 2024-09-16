//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

namespace GAP;

internal static class MLGenControl {
    private static readonly string ML_GEN_SCRIPT_NAME = "MLGenerator.py";
    private static readonly object[] TRAINING_DATA = [];

    // passes the training data to the network and returns the output
    internal static void RunGenerator() {
        string args = "";

        string output = PythonWrapper.RunPythonScript(ML_GEN_SCRIPT_NAME, args);

        Console.WriteLine(output);
    }
}
