//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

namespace GAP;

internal static class DeepDream {
    private static readonly bool VERBOSE = true;            // print interim results during the training process
    private static readonly float LEARNING_RATE = 20.0f;    // rate at which weights are tweaked and shifted
    private static readonly int NUM_OCTAVE = 3;             // number of octaves (enlarging + dream effect)
    private static readonly float OCTAVE_SCALE = 1.4f;      // how much the image gets resized, e.g. 1.4f => 40 % larger
    private static readonly int ITERATIONS = 30;            // number of iterations per octave
    private static readonly int MAX_LOSS = int.MaxValue;    // maximum loss cap per octave

    private static readonly string SCRIPT = @"deepdream\DeepDream.py";

    private static void WriteParameters() {
        // TODO - write parameters into a file
    }

    internal static void RunGenerator()
        => PythonWrapper.RunPythonScript(SCRIPT);
}
