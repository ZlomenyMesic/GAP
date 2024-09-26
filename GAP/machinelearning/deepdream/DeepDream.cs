//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

using GAP.util.exceptions;

namespace GAP;

internal static class DeepDream {
    // not necessary when using a file path, however, when using
    // an URL, image name has to be specified, e. g. "image.jpg"
    internal static string ImageName { get; set; } = "input.jpg";


    // the origin of the image; can either be a file path (both
    // absolute and relative) or an URL link (both including name)
    // e. g. "https://img-datasets.s3.amazonaws.com/coast.jpg"
    internal static string ImageOrigin { get; set; } = @"..\..\..\machinelearning\deepdream\input.jpg";


    // path where the result image is stored
    internal static string OutputPath { get; set; } = @"..\..\..\machinelearning\deepdream\output\dream.png";


    // format of the image origin:
    // 0 = file path; 1 = URL link
    internal static int ImageOriginFormat { get; set; } = 0;


    // print intermediate results during the training process
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

    // ALL_LAYERS - all 311 layers
    // ALMOST_FINAL_LAYERS - 23 selected good looking layers preferred to be closed to the end of the sequence
    // FINAL_LAYERS - 6 most interesting layers placed at the end of the sequence for the best result
    // UGLY_LAYERS - 10 terrible looking layers that are removed from ALL_LAYERS to avoid ugly outcomes
    internal static readonly string[] ALL_LAYERS = ["input_layer", "conv2d", "batch_normalization", "activation", "conv2d_1", "batch_normalization_1", "activation_1", "conv2d_2", "batch_normalization_2", "activation_2", "max_pooling2d", "conv2d_3", "batch_normalization_3", "activation_3", "conv2d_4", "batch_normalization_4", "activation_4", "max_pooling2d_1", "conv2d_8", "batch_normalization_8", "activation_8", "conv2d_6", "conv2d_9", "batch_normalization_6", "batch_normalization_9", "activation_6", "activation_9", "average_pooling2d", "conv2d_5", "conv2d_7", "conv2d_10", "conv2d_11", "batch_normalization_5", "batch_normalization_7", "batch_normalization_10", "batch_normalization_11", "activation_5", "activation_7", "activation_10", "activation_11", "mixed0", "conv2d_15", "batch_normalization_15", "activation_15", "conv2d_13", "conv2d_16", "batch_normalization_13", "batch_normalization_16", "activation_13", "activation_16", "average_pooling2d_1", "conv2d_12", "conv2d_14", "conv2d_17", "conv2d_18", "batch_normalization_12", "batch_normalization_14", "batch_normalization_17", "batch_normalization_18", "activation_12", "activation_14", "activation_17", "activation_18", "mixed1", "conv2d_22", "batch_normalization_22", "activation_22", "conv2d_20", "conv2d_23", "batch_normalization_20", "batch_normalization_23", "activation_20", "activation_23", "average_pooling2d_2", "conv2d_19", "conv2d_21", "conv2d_24", "conv2d_25", "batch_normalization_19", "batch_normalization_21", "batch_normalization_24", "batch_normalization_25", "activation_19", "activation_21", "activation_24", "activation_25", "mixed2", "conv2d_27", "batch_normalization_27", "activation_27", "conv2d_28", "batch_normalization_28", "activation_28", "conv2d_26", "conv2d_29", "batch_normalization_26", "batch_normalization_29", "activation_26", "activation_29", "max_pooling2d_2", "mixed3", "conv2d_34", "batch_normalization_34", "activation_34", "conv2d_35", "batch_normalization_35", "activation_35", "conv2d_31", "conv2d_36", "batch_normalization_31", "batch_normalization_36", "activation_31", "activation_36", "conv2d_32", "conv2d_37", "batch_normalization_32", "batch_normalization_37", "activation_32", "activation_37", "average_pooling2d_3", "conv2d_30", "conv2d_33", "conv2d_38", "conv2d_39", "batch_normalization_30", "batch_normalization_33", "batch_normalization_38", "batch_normalization_39", "activation_30", "activation_33", "activation_38", "activation_39", "mixed4", "conv2d_44", "batch_normalization_44", "activation_44", "conv2d_45", "batch_normalization_45", "activation_45", "conv2d_41", "conv2d_46", "batch_normalization_41", "batch_normalization_46", "activation_41", "activation_46", "conv2d_42", "conv2d_47", "batch_normalization_42", "batch_normalization_47", "activation_42", "activation_47", "average_pooling2d_4", "conv2d_40", "conv2d_43", "conv2d_48", "conv2d_49", "batch_normalization_40", "batch_normalization_43", "batch_normalization_48", "batch_normalization_49", "activation_40", "activation_43", "activation_48", "activation_49", "mixed5", "conv2d_54", "batch_normalization_54", "activation_54", "conv2d_55", "batch_normalization_55", "activation_55", "conv2d_51", "conv2d_56", "batch_normalization_51", "batch_normalization_56", "activation_51", "activation_56", "conv2d_52", "conv2d_57", "batch_normalization_52", "batch_normalization_57", "activation_52", "activation_57", "average_pooling2d_5", "conv2d_50", "conv2d_53", "conv2d_58", "conv2d_59", "batch_normalization_50", "batch_normalization_53", "batch_normalization_58", "batch_normalization_59", "activation_50", "activation_53", "activation_58", "activation_59", "mixed6", "conv2d_64", "batch_normalization_64", "activation_64", "conv2d_65", "batch_normalization_65", "activation_65", "conv2d_61", "conv2d_66", "batch_normalization_61", "batch_normalization_66", "activation_61", "activation_66", "conv2d_62", "conv2d_67", "batch_normalization_62", "batch_normalization_67", "activation_62", "activation_67", "average_pooling2d_6", "conv2d_60", "conv2d_63", "conv2d_68", "conv2d_69", "batch_normalization_60", "batch_normalization_63", "batch_normalization_68", "batch_normalization_69", "activation_60", "activation_63", "activation_68", "activation_69", "mixed7", "conv2d_72", "batch_normalization_72", "activation_72", "conv2d_73", "batch_normalization_73", "activation_73", "conv2d_70", "conv2d_74", "batch_normalization_70", "batch_normalization_74", "activation_70", "activation_74", "conv2d_71", "conv2d_75", "batch_normalization_71", "batch_normalization_75", "activation_71", "activation_75", "max_pooling2d_3", "mixed8", "conv2d_80", "batch_normalization_80", "activation_80", "conv2d_77", "conv2d_81", "batch_normalization_77", "batch_normalization_81", "activation_77", "activation_81", "conv2d_78", "conv2d_79", "conv2d_82", "conv2d_83", "average_pooling2d_7", "conv2d_76", "batch_normalization_78", "batch_normalization_79", "batch_normalization_82", "batch_normalization_83", "conv2d_84", "batch_normalization_76", "activation_78", "activation_79", "activation_82", "activation_83", "batch_normalization_84", "activation_76", "mixed9_0", "concatenate", "activation_84", "mixed9", "conv2d_89", "batch_normalization_89", "activation_89", "conv2d_86", "conv2d_90", "batch_normalization_86", "batch_normalization_90", "activation_86", "activation_90", "conv2d_87", "conv2d_88", "conv2d_91", "conv2d_92", "average_pooling2d_8", "conv2d_85", "batch_normalization_87", "batch_normalization_88", "batch_normalization_91", "batch_normalization_92", "conv2d_93", "batch_normalization_85", "activation_87", "activation_88", "activation_91", "activation_92", "batch_normalization_93", "activation_85", "mixed9_1", "concatenate_1", "activation_93", "mixed10"];
    internal static readonly string[] ALMOST_FINAL_LAYERS = ["activation_19", "activation_22", "activation_23", "activation_26", "activation_27", "activation_30", "activation_40", "activation_49", "activation_50", "activation_51", "batch_normalization_28", "batch_normalization_29", "batch_normalization_30", "batch_normalization_31", "batch_normalization_34", "batch_normalization_39", "conv2d_22", "conv2d_28", "conv2d_29", "conv2d_30", "mixed2", "mixed3", "mixed5"];
    internal static readonly string[] FINAL_LAYERS = ["mixed2", "mixed3", "mixed5", "activation_19", "activation_26", "activation_30"];
    internal static readonly string[] UGLY_LAYERS = ["activation", "activation_1", "activation_2", "batch_normalization", "batch_normalization_1", "batch_normalization_2", "conv2d", "conv2d_1", "conv2d_2", "input_layer"];

    // current layer name in the sequence
    private static string CurrentLayer { get; set; } = "mixed0";
    // current layer's activation
    private static int CurrentLayerActivation { get; set; } = 15;
    // current iteration in the sequence
    private static int CurrentIterationNumber { get; set; } = 0;


    private static readonly string PARAMS = @"..\..\..\machinelearning\deepdream\params.txt";
    private static readonly string SCRIPT = @"deepdream\DeepDream.py";
    private static readonly string DONE   = @"..\..\..\machinelearning\deepdream\output\";


    // save the parameters into params.txt to make them accessible from python
    // changing the order also has to be done in DeepDream.py
    private static void SaveParameters(string inputName, string inputOrigin, int inputOriginFormat, string outputPath) {
        using StreamWriter sw = new(PARAMS);
        sw.Write($"{inputName}\n{inputOrigin}\n{inputOriginFormat}\n{outputPath}\n{Verbose}\n{DistortionRate}\n{Octaves}\n{OctaveScale}\n{Iterations}\n{MaxLoss}\n{CurrentLayer}\n{CurrentLayerActivation}\n{CurrentIterationNumber}");
    }

    // simple wrapper function
    private static void RunGenerator(bool firstIteration) {
        // input for the first layer is the input image
        // input for all oncoming layer is the previous layer's output
        SaveParameters(
            firstIteration ? ImageName : "dream.png",
            firstIteration ? ImageOrigin : OutputPath,
            firstIteration ? ImageOriginFormat : 0,
            OutputPath);

        PythonWrapper.RunPythonScript(SCRIPT);
    }

    // some layers are removed as they look terrible
    private static IEnumerable<string> CleanLayers() {
        foreach (string layer in ALL_LAYERS) {
            if (!UGLY_LAYERS.Contains(layer))
                yield return layer;
        }
    }

    // waits for a file named after the current iteration
    // this file is created by the python script after saving the dream image
    private static void WaitForPy(int i) {
        string done = $"{DONE}{i + 1}";
        while (!File.Exists(done))
            Thread.Sleep(500);
        File.Delete(done);
    }

    // transforms the input image using randomly selected layers
    internal static void RunGeneratorRandom(int layers) {
        if (layers <= 0) throw new Exception();

        List<string> allLayers = CleanLayers().ToList();

        Random r = new();
        for (int i = 0; i < layers; i++) {
            CurrentIterationNumber = i + 1;

            // 1. all layers except the last two are chosen randomly from the ALL_LAYERS - UGLY_LAYERS array
            // 2. the second to last layer is only selected from the ALMOST_FINAL_LAYERS array
            // 3. the last layer gets chosen from the FINAL_LAYERS array
            // the reason for shortening the last two selections is that most of the layers are too repetitive,
            // periodical, boring or just not very good looking. this ensures the final result looks interesting
            CurrentLayer = i < layers - 2
                ? allLayers[r.Next(0, allLayers.Count)]
                : i != layers - 1
                    ? ALMOST_FINAL_LAYERS[r.Next(0, ALMOST_FINAL_LAYERS.Length)] 
                    : FINAL_LAYERS[r.Next(0, FINAL_LAYERS.Length)];

            // all layers get activation 6, last layer gets 15;
            // if all layers got more activation, the result would get too crammed
            CurrentLayerActivation = i == layers - 1 ? 15 : 6;

            RunGenerator(i == 0);
            WaitForPy(i);
        }
    }

    // transforms the input image using custom specified layers
    internal static void RunGeneratorCustom(params string[] layers) {
        if (layers.Length <= 0) throw new UnknownLayerException($"deepdream cannot be run: {layers.Length} layers passed");
        foreach (string layer in layers) {
            if (!ALL_LAYERS.Contains(layer))
                throw new UnknownLayerException($"deepdream cannot be run: layer {layer} not found");
        }

        // identical process as described above
        for (int i = 0; i < layers.Length; i++) {
            CurrentIterationNumber = i + 1;
            CurrentLayerActivation = i == layers.Length - 1 ? 15 : 6;
            CurrentLayer = layers[i];

            RunGenerator(i == 0);
            WaitForPy(i);
        }
    }
}
