//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

using System.Drawing;
using GAP.core.image.generation;
using GAP.core.modLoader;
using NeoKolors.Console;
using NeoKolors.Settings;

namespace GAP.machineLearning.ladybug;

[ExcludeFromModLoading]
public class Ladybug : IImageGenerator<Ladybug> {
    private const string TRAINING_SCRIPT_PATH = @"ladybug\ModelTraining.py";
    private const string APPLICATION_SCRIPT_PATH = @"ladybug\ModelApplication.py";

    internal static void TrainModel() {
        PythonWrapper.RunPythonScript(TRAINING_SCRIPT_PATH);
        WaitForOutput();
    }

    internal static void RunGenerator() {
        PythonWrapper.RunPythonScript(APPLICATION_SCRIPT_PATH);
        WaitForOutput();
    }
    private static void WaitForOutput() {
        while (!File.Exists("DONE"))
            Thread.Sleep(500);
        File.Delete("DONE");
    }

    public Bitmap GenerateImage() {
        TrainModel();
        RunGenerator();
        
        try {
            var bmp = Image.FromFile("./../../../machinelearning/ladybug/output/output.png");
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
    public SettingsBuilder<Ladybug> GetSettings() => SettingsBuilder<Ladybug>.Build("ladybug");

    ISettingsBuilder<IImageGenerator> IImageGenerator.GetSettings() => GetSettings();
}
