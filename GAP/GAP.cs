//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

using GAP.core.modLoader;
using GAP.machineLearning.deepdream;
using Kolors;

namespace GAP;

internal class GAP : Mod {

    // constants
    public const string PROJECT_ID = "gap";
    private const string DESCRIPTION = "official vanilla GAP generators";
    private const Debug.DebugLevel DEBUG_LEVEL = Debug.DebugLevel.ALL;
    private static readonly string[] EXCLUDED_BINARIES = [
        "Kolors", 
        "Microsoft.Win32.SystemEvents",
        "System.Drawing.Common"
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
        // Totally harmless, and totally doesn't load itself as a mod
        //int modCount = ModLoader.LoadMods(".");

        ModLoader.LoadMods(".", EXCLUDED_BINARIES);
        ModLoader.WriteRegisteredMods();





        DeepDream.Iterations = 5;
        DeepDream.Octaves = 8;
        DeepDream.LayerActivationFunction = DeepDream.LayerActivationFunctions.LastPriority;
        DeepDream.RunGeneratorFilteredRandom(3);

        //DeepDream.RunGeneratorCustom("conv2d_26", "activation_26", "activation_19");

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