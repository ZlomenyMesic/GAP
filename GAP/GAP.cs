//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

using System.Drawing;
using System.Globalization;
using GAP;
using GAP.core.image.generation.generators;
using GAP.core.image.transformation;
using GAP.core.image.transformation.transformers;
using GAP.core.modLoader;
using GAP.util;
using GAP.util.settings;
using GAP.util.settings.argumentType;
using Kolors;
using static GAP.util.settings.Arguments;

namespace GAP;

class GAP : Mod {

    // constants
    
    public const string PROJECT_ID = "gap";
    public const string DESCRIPTION = "official vanilla GAP generators";
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
        
        Console.Title = "GAP: cli";
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        
        Debug.debugLevel = DEBUG_LEVEL;
        
        // Mod loading
        int modCount = ModLoader.LoadMods(".", EXCLUDED_BINARIES);
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