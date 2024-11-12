//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

using GAP.core.modLoader;
using Kolors;
using GAP.machineLearning.deepdream;
using GAP.machineLearning.ladybug;

namespace GAP;

internal class GAP : Mod {

    // constants
    public const string PROJECT_ID = "gap";
    private const string DESCRIPTION = "official vanilla GAP generators";
    private const Debug.DebugLevel DEBUG_LEVEL = Debug.DebugLevel.ALL;
    private static readonly string[] EXCLUDED_BINARIES = [
        "Kolors", 
        "Microsoft.Win32.SystemEvents",
        "System.Drawing.Common",
        "NAudio.Asio",
        "NAudio.Core",
        "NAudio",
        "NAudio.Midi",
        "NAudio.Wasapi",
        "NAudio.WinMM"
    ];

    //
    // NOTE:
    // to count lines in project call " git ls-files | xargs wc -l " in a linux shell
    // for a specific file type " git ls-files | grep '\.<file-extension>' | xargs wc -l "
    //
    
    //
    // TODOS:
    //
    //
    
    static int Main() {
        
        
        // --- MAIN FUNCTIONALITY ---
        // DO NOT REMOVE !!!
        
        // System.Drawing.Common is only for Windows
        // this is necessary for not crashing somewhere else and
        // debugging it while having no idea wtf is it doing 
        if (!OperatingSystem.IsWindows()) {
            throw new PlatformNotSupportedException("Only Windows platforms are supported.");
        }
        
        // Console settings
        Console.Title = "GAP: cli";
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        
        // Debug levels (maybe change this to no info later?)
        Debug.debugLevel = DEBUG_LEVEL;
        
        // Mod loading
        ModLoader.LoadMods(".", EXCLUDED_BINARIES);
        ModLoader.WriteRegisteredMods();

        // --- END OF MAIN FUNCTIONALITY ---
        // you can put your code here:

        //DeepDream.ImageOrigin = @"C:\Users\michn\Desktop\convolutions\outputs\dog1.png";
        //DeepDream.OctaveScale = 1.2f;
        //DeepDream.RunGeneratorFilteredRandom(3);

        //Ladybug.RenameData();
        //Ladybug.CreateDataSubsets();
        //Ladybug.TrainModel();
        //Ladybug.RunGenerator();

        return 0;
    }

    public string Register() {
        return PROJECT_ID;
    }

    public void Initialize() {
        DefaultRegistries.Register();
    }

    public string GetInfo() {
        return DESCRIPTION;
    }
}