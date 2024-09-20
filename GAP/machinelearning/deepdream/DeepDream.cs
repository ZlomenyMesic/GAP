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
    internal static string ImageName { get; set; } = "input.jpg";


    // the origin of the image; can either be a file path (both
    // absolute and relative) or an URL link (both including name)
    // e. g. "https://img-datasets.s3.amazonaws.com/coast.jpg"
    internal static string ImageOrigin { get; set; } = @"..\..\..\machinelearning\deepdream\input.jpg";


    // format of the image origin:
    // 0 = file path; 1 = URL link
    internal static int ImageOriginFormat { get; set; } = 0;


    // print interim results during the training process, includes
    // current octave and iteration info + current loss value
    internal static bool Verbose { get; set; } = true;


    // rate at which the image gets distorted
    internal static float DistortionRate { get; set; } = 20.0f;


    // total number of octaves (loops), each octave
    // involves image resizing + adding "dream effect"
    internal static int Octaves { get; set; } = 8;


    // ration of image resizing, e. g. 1.4f => resolution 40 % larger
    internal static float OctaveScale { get; set; } = 1.3f;


    // number of iterations per octave
    internal static int Iterations { get; set; } = 5;


    // maximum loss cap per octave, ensures the dream effect doesn't
    // get too strong; for better effect, no cap is recommended
    internal static int MaxLoss { get; set; } = int.MaxValue;


    // activations of different layers
    // 
    internal static float Mixed0 { get; set; } = 12.0f;
    internal static float Mixed1 { get; set; } = 8.0f;
    internal static float Mixed2 { get; set; } = 3.0f;
    internal static float Mixed3 { get; set; } = 2.0f;
    internal static float Mixed4 { get; set; } = 2.0f;
    internal static float Mixed5 { get; set; } = 2.5f;
    internal static float Mixed6 { get; set; } = 3.5f;
    internal static float Mixed7 { get; set; } = 5.5f;
    internal static float Mixed8 { get; set; } = 8.0f;
    internal static float Mixed9 { get; set; } = 15.0f;
    internal static float Mixed10 { get; set; } = 25.0f;


    private static readonly string PARAMS = @"..\..\..\machinelearning\deepdream\params.txt";
    private static readonly string SCRIPT = @"deepdream\DeepDream.py";


    // save the parameters into params.txt to make them accessible from
    // python; changing the order also has to be done in DeepDream.py
    private static void SaveParameters() {
        using StreamWriter sw = new(PARAMS);
        sw.Write($"{ImageName}\n{ImageOrigin}\n{ImageOriginFormat}\n{Verbose}\n{DistortionRate}\n{Octaves}\n{OctaveScale}\n{Iterations}\n{MaxLoss}");
    }

    // simple wrapper function
    internal static void RunGenerator() {
        SaveParameters();
        PythonWrapper.RunPythonScript(SCRIPT);
    }
}
