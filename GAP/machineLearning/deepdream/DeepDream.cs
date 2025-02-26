//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 
//      founded 11.9.2024
//

using System.Diagnostics;
using System.Drawing;
using System.Text;
using GapCore;
using GapCore.modLoader;
using GapCore.util.exceptions;
using NeoKolors.Settings;
using Debug = NeoKolors.Console.Debug;

namespace GAP.machineLearning.deepdream;


/// <summary>
/// 
/// CHANGEABLE PARAMETERS:
/// 
///   Verbose
///   ImageName
///   ImageOrigin
///   ImageOriginFormat
///   OutputPath
///   Octaves
///   OctaveScale
///   Iterations
///   DistortionRate
///   MaxLoss
///   LayerSequence
///   LayerActivationFunction
///  
/// HOW TO RUN THE GENERATOR?
/// use one of the following functions:
/// 
///   RunGenerator()
///   RunGeneratorCustom()
///   RunGeneratorRandom()
///   RunGeneratorFilteredRandom()
///  
/// </summary>
[ExcludeFromModLoading]
public class DeepDream : IImageGenerator<DeepDream> {

    /// <summary>
    /// print interim stats or other information during the process
    /// </summary>
    public bool Verbose { get; set; } = true;


    /// <summary>
    /// not necessary when using a file path. however, when using
    /// a URL, image name has to be specified, e.g. "image.jpg"
    /// </summary>
    public string ImageName { get; set; } = "input.jpg";


    /// <summary>
    /// the origin of the image; can either be a file path (both
    /// absolute or relative) or an URL (both including image name)
    /// e.g. "https://img-datasets.s3.amazonaws.com/coast.jpg"
    /// </summary>
    public string ImageOrigin { get; set; } = @"..\..\..\machineLearning\deepdream\input.jpg";


    /// <summary>
    /// format/type of the image origin:
    /// 0 = absolute/relative file path
    /// 1 = URL link
    /// </summary>
    public ImageSource ImageOriginFormat { get; set; } = 0;

    /// <summary>
    /// source type of the input image
    /// </summary>
    public enum ImageSource {
        FILE_PATH = 0,
        URL = 1
    }

    /// <summary>
    /// location where the output image will be saved
    /// </summary>
    public string OutputPath { get; set; } = @"..\..\..\machineLearning\deepdream\output\dream.png";


    /// <summary>
    /// total number of octaves (loops) per layer. each octave
    /// involves resizing the image + adding the "dream effect".
    /// see our paper for further information and understanding
    /// </summary>
    public int Octaves { get; set; } = 8;


    /// <summary>
    /// ration of image resizing each octave
    /// e.g. 1.4f => resolution is increased by 40 % each octave
    /// </summary>
    public float OctaveScale { get; set; } = 1.3f;


    /// <summary>
    /// number of iterations (steps) in each gradient ascent loop
    /// there is a single gradient ascent loop per octave
    /// </summary>
    public int Iterations { get; set; } = 5;


    /// <summary>
    /// rate at which the image gets modified/distorted
    /// </summary>
    public float DistortionRate { get; set; } = 20.0f;


    /// <summary>
    /// maximum loss cap per gradient ascent loop. ensures the dream
    /// effect doesn't get too strong. for the best result, we don't
    /// recommend setting a cap
    /// </summary>
    public int MaxLoss { get; set; } = int.MaxValue;


    /// <summary>
    /// all 311 layers from the InceptionV3 model
    /// </summary>
    public static readonly string[] ALL_LAYERS = ["input_layer", "conv2d", "batch_normalization", "activation", "conv2d_1", "batch_normalization_1", "activation_1", "conv2d_2", "batch_normalization_2", "activation_2", "max_pooling2d", "conv2d_3", "batch_normalization_3", "activation_3", "conv2d_4", "batch_normalization_4", "activation_4", "max_pooling2d_1", "conv2d_8", "batch_normalization_8", "activation_8", "conv2d_6", "conv2d_9", "batch_normalization_6", "batch_normalization_9", "activation_6", "activation_9", "average_pooling2d", "conv2d_5", "conv2d_7", "conv2d_10", "conv2d_11", "batch_normalization_5", "batch_normalization_7", "batch_normalization_10", "batch_normalization_11", "activation_5", "activation_7", "activation_10", "activation_11", "mixed0", "conv2d_15", "batch_normalization_15", "activation_15", "conv2d_13", "conv2d_16", "batch_normalization_13", "batch_normalization_16", "activation_13", "activation_16", "average_pooling2d_1", "conv2d_12", "conv2d_14", "conv2d_17", "conv2d_18", "batch_normalization_12", "batch_normalization_14", "batch_normalization_17", "batch_normalization_18", "activation_12", "activation_14", "activation_17", "activation_18", "mixed1", "conv2d_22", "batch_normalization_22", "activation_22", "conv2d_20", "conv2d_23", "batch_normalization_20", "batch_normalization_23", "activation_20", "activation_23", "average_pooling2d_2", "conv2d_19", "conv2d_21", "conv2d_24", "conv2d_25", "batch_normalization_19", "batch_normalization_21", "batch_normalization_24", "batch_normalization_25", "activation_19", "activation_21", "activation_24", "activation_25", "mixed2", "conv2d_27", "batch_normalization_27", "activation_27", "conv2d_28", "batch_normalization_28", "activation_28", "conv2d_26", "conv2d_29", "batch_normalization_26", "batch_normalization_29", "activation_26", "activation_29", "max_pooling2d_2", "mixed3", "conv2d_34", "batch_normalization_34", "activation_34", "conv2d_35", "batch_normalization_35", "activation_35", "conv2d_31", "conv2d_36", "batch_normalization_31", "batch_normalization_36", "activation_31", "activation_36", "conv2d_32", "conv2d_37", "batch_normalization_32", "batch_normalization_37", "activation_32", "activation_37", "average_pooling2d_3", "conv2d_30", "conv2d_33", "conv2d_38", "conv2d_39", "batch_normalization_30", "batch_normalization_33", "batch_normalization_38", "batch_normalization_39", "activation_30", "activation_33", "activation_38", "activation_39", "mixed4", "conv2d_44", "batch_normalization_44", "activation_44", "conv2d_45", "batch_normalization_45", "activation_45", "conv2d_41", "conv2d_46", "batch_normalization_41", "batch_normalization_46", "activation_41", "activation_46", "conv2d_42", "conv2d_47", "batch_normalization_42", "batch_normalization_47", "activation_42", "activation_47", "average_pooling2d_4", "conv2d_40", "conv2d_43", "conv2d_48", "conv2d_49", "batch_normalization_40", "batch_normalization_43", "batch_normalization_48", "batch_normalization_49", "activation_40", "activation_43", "activation_48", "activation_49", "mixed5", "conv2d_54", "batch_normalization_54", "activation_54", "conv2d_55", "batch_normalization_55", "activation_55", "conv2d_51", "conv2d_56", "batch_normalization_51", "batch_normalization_56", "activation_51", "activation_56", "conv2d_52", "conv2d_57", "batch_normalization_52", "batch_normalization_57", "activation_52", "activation_57", "average_pooling2d_5", "conv2d_50", "conv2d_53", "conv2d_58", "conv2d_59", "batch_normalization_50", "batch_normalization_53", "batch_normalization_58", "batch_normalization_59", "activation_50", "activation_53", "activation_58", "activation_59", "mixed6", "conv2d_64", "batch_normalization_64", "activation_64", "conv2d_65", "batch_normalization_65", "activation_65", "conv2d_61", "conv2d_66", "batch_normalization_61", "batch_normalization_66", "activation_61", "activation_66", "conv2d_62", "conv2d_67", "batch_normalization_62", "batch_normalization_67", "activation_62", "activation_67", "average_pooling2d_6", "conv2d_60", "conv2d_63", "conv2d_68", "conv2d_69", "batch_normalization_60", "batch_normalization_63", "batch_normalization_68", "batch_normalization_69", "activation_60", "activation_63", "activation_68", "activation_69", "mixed7", "conv2d_72", "batch_normalization_72", "activation_72", "conv2d_73", "batch_normalization_73", "activation_73", "conv2d_70", "conv2d_74", "batch_normalization_70", "batch_normalization_74", "activation_70", "activation_74", "conv2d_71", "conv2d_75", "batch_normalization_71", "batch_normalization_75", "activation_71", "activation_75", "max_pooling2d_3", "mixed8", "conv2d_80", "batch_normalization_80", "activation_80", "conv2d_77", "conv2d_81", "batch_normalization_77", "batch_normalization_81", "activation_77", "activation_81", "conv2d_78", "conv2d_79", "conv2d_82", "conv2d_83", "average_pooling2d_7", "conv2d_76", "batch_normalization_78", "batch_normalization_79", "batch_normalization_82", "batch_normalization_83", "conv2d_84", "batch_normalization_76", "activation_78", "activation_79", "activation_82", "activation_83", "batch_normalization_84", "activation_76", "mixed9_0", "concatenate", "activation_84", "mixed9", "conv2d_89", "batch_normalization_89", "activation_89", "conv2d_86", "conv2d_90", "batch_normalization_86", "batch_normalization_90", "activation_86", "activation_90", "conv2d_87", "conv2d_88", "conv2d_91", "conv2d_92", "average_pooling2d_8", "conv2d_85", "batch_normalization_87", "batch_normalization_88", "batch_normalization_91", "batch_normalization_92", "conv2d_93", "batch_normalization_85", "activation_87", "activation_88", "activation_91", "activation_92", "batch_normalization_93", "activation_85", "mixed9_1", "concatenate_1", "activation_93", "mixed10"];

    /// <summary>
    /// 23 good-looking layer preferred to be the second to last layer
    /// in the sequence. used in RunGeneratorFilteredRandom()
    /// </summary>
    public static readonly string[] ALMOST_FINAL_LAYERS = ["activation_19", "activation_22", "activation_23", "activation_26", "activation_27", "activation_30", "activation_40", "activation_49", "activation_50", "activation_51", "batch_normalization_28", "batch_normalization_29", "batch_normalization_30", "batch_normalization_31", "batch_normalization_34", "batch_normalization_39", "conv2d_22", "conv2d_28", "conv2d_29", "conv2d_30", "mixed2", "mixed3", "mixed5"];

    /// <summary>
    /// 6 most interesting layers from which the last layer is selected.
    /// also used by RunGeneratorFilteredCustom()
    /// </summary>
    public static readonly string[] FINAL_LAYERS = ["mixed2", "mixed3", "mixed5", "activation_19", "activation_26", "activation_30"];

    /// <summary>
    /// 10 terrible looking layers that are removed from the layer selection.
    /// also used only in RunGeneratorFilteredRandom()
    /// (these layers include either really boring or single color effects)
    /// </summary>
    public static readonly string[] UGLY_LAYERS = ["activation", "activation_1", "activation_2", "batch_normalization", "batch_normalization_1", "batch_normalization_2", "conv2d", "conv2d_1", "conv2d_2", "input_layer"];

    /// <summary>
    /// the current sequence of layers used in the generator
    /// </summary>
    public object[] LayerSequence { get; set; } = ["mixed0"];


    public string FinishCheckPath { get; set; } = @"..\..\..\machineLearning\deepdream\output\DONE";


    /// <summary>
    /// different possible layer activation functions
    /// any custom function must take the current iteration and total number of layers as arguments
    /// </summary>
    public static class LayerActivationFunctions {
        /// <summary>
        /// all steps are equal to 10
        /// </summary>
        public static readonly Func<int, int, int> CONSTANT = (_, _) => 10;
        /// <summary>
        /// all steps except the last one are equal to 5, last one is equal to 15
        /// </summary>
        public static readonly Func<int, int, int> LAST_PRIORITY = (iteration, count) => iteration == count - 1 ? 15 : 5;
        /// <summary>
        /// linear function starting at 5 and increasing by one each step
        /// </summary>
        public static readonly Func<int, int, int> LINEAR_INCREASING = (iteration, _) => 4 + iteration;
        /// <summary>
        /// linear function decreasing by one each step and ending at 5
        /// </summary>
        public static readonly Func<int, int, int> LINEAR_DECREASING = (iteration, count) => 5 + count - iteration;
        /// <summary>
        /// 2 raised to the power of the current step
        /// !!! TERRIBLE OPTION FOR MORE LAYERS !!!
        /// </summary>
        public static readonly Func<int, int, int> QUADRATIC = (iteration, _) => (int)Math.Pow(2, iteration);
    }
    
    /// <summary>
    /// currently used layer activation function
    /// </summary>
    public Func<int, int, int> LayerActivationFunction { get; set; } = LayerActivationFunctions.LAST_PRIORITY;


    private const string PARAMS_PATH = @"..\..\..\machineLearning\deepdream\params.txt";
    private const string DD_PYTHON_SCRIPT_PATH = @"deepdream\DeepDream.py";


    /// <summary>
    /// saves the current parameters to params.txt to make them accessible from
    /// the python script. changing the order also has to be done in DeepDream.py
    /// </summary>
    private void SaveParameters() {
        // convert the layer sequence and the respective activations into two single line strings
        string layers = ArrayToString(LayerSequence);
        string activations = ArrayToString([..CreateLayerActivationsArray(LayerSequence.Length)]);

        // save the parameters to the file
        using StreamWriter sw = new(PARAMS_PATH);
        sw.Write($"{Verbose}\n{ImageName}\n{ImageOrigin}\n{ImageOriginFormat}\n{OutputPath}\n{FinishCheckPath}\n{DistortionRate}\n{Octaves}\n{OctaveScale}\n{Iterations}\n{MaxLoss}\n{layers}\n{activations}");
    }


    /// <summary>
    /// creates an array of successive layer activations base on LayerActivationFunction
    /// </summary>
    /// <param name="length">number of all layers</param>
    /// <returns>array of layer activations</returns>
    private int[] CreateLayerActivationsArray(int length) {
        int[] activations = new int[length];
        for (int i = 0; i < length; i++) {
            activations[i] = LayerActivationFunction(i, length);
        }
        return activations;
    }


    /// <summary>
    /// puts all array items separated by spaces into a single line string
    /// </summary>
    /// <param name="arr">array to convert</param>
    /// <returns>single line string</returns>
    private static string ArrayToString(object[] arr) {
        StringBuilder sb = new();
        for (int i = 0; i < arr.Length; i++) {
            sb.Append($"{(i != 0 ? " " : "")}{arr[i]}");
        }
        return sb.ToString();
    }


    /// <summary>
    /// removes UGLY_LAYERS from ALL_LAYERS
    /// </summary>
    /// <returns>all layers except for the ugly ones</returns>
    private static IEnumerable<string> CleanLayers() {
        foreach (string layer in ALL_LAYERS) {
            if (!UGLY_LAYERS.Contains(layer))
                yield return layer;
        }
    }


    /// <summary>
    /// waits for a file called DONE. this file is created by the python
    /// script after finishing the generation and saving the output image
    /// </summary>
    private void WaitForOutput() {
        while (!File.Exists(FinishCheckPath))
            Thread.Sleep(500);
        File.Delete(FinishCheckPath);
    }


    /// <summary>
    /// runs the generator with the current layer sequence
    /// </summary>
    public void RunGenerator() {
        // measure the time spent to generate the image
        Stopwatch timer = new();
        timer.Start();

        // in case of a bug
        File.Delete(FinishCheckPath);

        SaveParameters();

        PythonWrapper.RunPythonScript(DD_PYTHON_SCRIPT_PATH);
        WaitForOutput();

        // print the time spent
        timer.Stop();
        if (Verbose) Console.WriteLine($"Time spent: {timer.ElapsedMilliseconds / 1000} s");
    }


    /// <summary>
    /// runs the generator with a specified layer sequence
    /// </summary>
    /// <param name="layers">layers to be used, in order</param>
    /// <exception cref="InvalidLayerException"></exception>
    public void RunGenerator(params string[] layers) {
        if (layers.Length <= 0) throw new InvalidLayerException(1, layers.Length.ToString());
        foreach (string layer in layers) {
            if (!ALL_LAYERS.Contains(layer))
                throw new InvalidLayerException(0, layer);
        }

        LayerSequence = [..layers];

        RunGenerator();
    }


    /// <summary>
    /// runs the generator with completely random layers. the output may not
    /// look very well. we highly encourage you to use the filtered version
    /// </summary>
    /// <param name="layers">number of layers</param>
    /// <exception cref="InvalidLayerException"></exception>
    public void RunGeneratorRandom(int layers) {
        if (layers <= 0) throw new InvalidLayerException(1, layers.ToString());

        string[] selectedLayers = new string[layers];

        Random r = new();
        for (int i = 0; i < layers; i++) {
            selectedLayers[i] = ALL_LAYERS[r.Next(0, ALL_LAYERS.Length)];
        }

        RunGenerator(selectedLayers);
    }


    /// <summary>
    /// runs the generator with an almost random layer sequence. a few ugly
    /// layers are removed as they could make the final output look bad,
    /// and the last two layers are only selected from the best looking ones
    /// </summary>
    /// <param name="layers">number of layers</param>
    /// <exception cref="InvalidLayerException"></exception>
    public void RunGeneratorFilteredRandom(int layers) {
        if (layers <= 0) throw new InvalidLayerException(1, layers.ToString());

        // remove the ugly layers
        List<string> allLayers = CleanLayers().ToList();

        string[] selectedLayers = new string[layers];

        Random r = new();
        for (int i = 0; i < layers; i++) {
            // 1. all layers except the last two are chosen randomly from the ALL_LAYERS - UGLY_LAYERS array
            // 2. the second to last layer is only selected from the ALMOST_FINAL_LAYERS array
            // 3. the last layer gets chosen from the FINAL_LAYERS array
            // the reason for shortening the last two selections is that most of the layers are too repetitive,
            // periodical, boring or just not very good looking. this ensures the final result looks interesting
            selectedLayers[i] = i < layers - 2
                ? allLayers[r.Next(0, allLayers.Count)]
                : i != layers - 1
                    ? ALMOST_FINAL_LAYERS[r.Next(0, ALMOST_FINAL_LAYERS.Length)]
                    : FINAL_LAYERS[r.Next(0, FINAL_LAYERS.Length)];
        }

        RunGenerator(selectedLayers);
    }

    
    public Bitmap GenerateImage() {
        try {
            var bmp = Image.FromFile(OutputPath);
            return (Bitmap)bmp;
        }
        catch (FileNotFoundException e) {
            Debug.Throw(e);
        }
        catch (ArgumentException e) {
            Debug.Throw(e);
        }

        return null!;
    }

    private static readonly SettingsBuilder<DeepDream> SETTING = SettingsBuilder<DeepDream>.Build("deep_dream",
        SettingsNode<DeepDream>
            .New("advanced")
            .Argument("verbose", Arguments.Bool())
            .Argument("image_name", Arguments.String(maxLength: 48, allowSpecial: false, allowNewlines: false))
            .Argument("image_origin", Arguments.String(maxLength: 512, allowNewlines: false))
            .Argument("image_origin_format", Arguments.SingleSelectEnum(ImageSource.FILE_PATH))
            .Argument("output_path", Arguments.String(maxLength: 512, allowNewlines: false))
            .Argument("octaves", Arguments.Integer(1, 10, 8))
            .Argument("octave_scale", Arguments.Float(1.0f, defaultValue: 1.3f))
            .Argument("iterations", Arguments.Integer(1, defaultValue: 5))
            .Argument("distortion_rate", Arguments.Float(defaultValue: 20))
            .Argument("max_loss", Arguments.Integer(1, defaultValue: int.MaxValue))
            .Argument("layer_sequence", Arguments.SelectionList(ALL_LAYERS))
            .Argument("finish_check_path", Arguments.String(maxLength: 512, allowNewlines: false))
            .Constructs(c => {
                var d = new DeepDream {
                    Verbose = (bool)c["verbose"].Get(),
                    ImageName = (string)c["image_name"].Get(),
                    ImageOrigin = (string)c["image_origin"].Get(),
                    ImageOriginFormat = (ImageSource)c["image_origin_format"].Get(),
                    OutputPath = (string)c["output_path"].Get(),
                    Octaves = (int)c["octaves"].Get(),
                    OctaveScale = (float)c["octave_scale"].Get(),
                    Iterations = (int)c["iterations"].Get(),
                    DistortionRate = (float)c["distortion_rate"].Get(),
                    MaxLoss = (int)c["max_loss"].Get(),
                    LayerSequence = ((object[])c["layer_sequence"].Get()).ToArray(),
                    FinishCheckPath = (string)c["finish_check_path"].Get()
                };

                return d;
            })
    );
    
    public SettingsBuilder<DeepDream> GetSettings() => (SettingsBuilder<DeepDream>)SETTING.Clone();
    ISettingsBuilder<IImageGenerator> IImageGenerator.GetSettings() => GetSettings();
}
