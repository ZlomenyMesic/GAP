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

    public const string PROJECT_ID = "gap";
    private const Debug.DebugLevel DEBUG_LEVEL = Debug.DebugLevel.ALL;

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
        
        Debug.debugLevel = DEBUG_LEVEL;

        // Mod loading
        // Totally harmless, and totally doesn't load itself as a mod
        int modCount = ModLoader.LoadMods(".");
        
        // foreach (var modName in ModLoader.GetRegisteredMods()) Debug.info($"Loaded mod '{modName}'");
        // Debug.info($"Loaded {modCount} mods total");
        
        WhiteNoise wn = new WhiteNoise();
        // Console.WriteLine(wn.GetSettings<WhiteNoise>());
        
        var settings = wn.GetSettings<WhiteNoise>();
        
        settings["advanced"].SetValue("width", 123);
        settings["advanced"].SetValue("height", 456);
        settings["advanced"].SetValue("seed", 789);
        settings["advanced"].SetGroupOptionValue("advanced", "presets", "presets", WhiteNoisePresets.GRAYSCALE);

        Console.WriteLine(settings.ToString());
        
        var whiteNoise = settings.Execute("advanced", ("advanced", "presets"));
        Bitmap bmp = whiteNoise.GenerateImage();
        bmp.Save("gap.png");
        
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
}