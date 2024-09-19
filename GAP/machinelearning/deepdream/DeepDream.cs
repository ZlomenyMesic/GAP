//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

namespace GAP;

internal static class DeepDream {
    // not necessary when using a file path, however, when using
    // an URL, image name has to be specified, e. g. "image.jpg"
    private static readonly string IMG_NAME = "input.jpg";


    // the origin of the image; can either be a file path (both
    // absolute and relative) or an URL link (both including name)
    // e. g. "https://img-datasets.s3.amazonaws.com/coast.jpg"
    private static readonly string IMG_ORIGIN = @"..\..\..\machinelearning\deepdream\input.jpg";


    // format of the image origin:
    // 0 = file path; 1 = URL link
    private static readonly int IMG_ORIGIN_FORMAT = 0;


    // print interim results during the training process, includes
    // current octave and iteration info + current loss value
    private static readonly bool VERBOSE = true;


    // rate at which the image gets distorted (theoretically not a "learning rate")
    private static readonly float LEARNING_RATE = 20.0f;


    // total number of octaves (loops), each octave
    // involves image resizing + adding "dream effect"
    private static readonly int OCTAVES = 8;


    // ration of image resizing, e. g. 1.4f => resolution 40 % larger
    private static readonly float OCTAVE_SCALE = 1.3f;


    // number of iterations per octave
    private static readonly int ITERATIONS = 35;


    // maximum loss cap per octave, ensures the dream effect doesn't
    // get too strong; for better effect, no cap is recommended
    private static readonly int MAX_LOSS = int.MaxValue;    // maximum loss cap per octave


    private static readonly string PARAMS = @"..\..\..\machinelearning\deepdream\params.txt";
    private static readonly string SCRIPT = @"deepdream\DeepDream.py";


    // save the parameters into params.txt to make them accessible from
    // python; changing the order also has to be done in DeepDream.py
    private static void SaveParameters() {
        using StreamWriter sw = new(PARAMS);
        sw.Write($"{IMG_NAME}\n{IMG_ORIGIN}\n{IMG_ORIGIN_FORMAT}\n{VERBOSE}\n{LEARNING_RATE}\n{OCTAVES}\n{OCTAVE_SCALE}\n{ITERATIONS}\n{MAX_LOSS}");
    }

    // simple wrapper function
    internal static void RunGenerator() {
        SaveParameters();
        PythonWrapper.RunPythonScript(SCRIPT);
    }
}
