//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

using GAP.core.modLoader;
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
    // slider argument type
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
        ModLoader.LoadMods(".", EXCLUDED_BINARIES);
        ModLoader.WriteRegisteredMods();
        
        // DeepDream.RunGeneratorRandom(4);
        // DeepDream.RunGeneratorCustom("mixed3", "mixed2", "mixed5");

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