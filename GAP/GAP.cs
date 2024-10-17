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
    public const Debug.DebugLevel DEBUG_LEVEL = Debug.DebugLevel.ALL;

    static int Main() {
        
        Debug.debugLevel = DEBUG_LEVEL;

        // Mod loading
        // Totally harmless, and totally doesn't load itself as a mod
        int modCount = ModLoader.LoadMods(".");
        
        // foreach (var modName in ModLoader.GetRegisteredMods()) Debug.info($"Loaded mod '{modName}'");
        // Debug.info($"Loaded {modCount} mods total");
        
        // SettingsBuilder<Grid> settingsBuilder = new SettingsBuilder<Grid>("grid");
        //
        // settingsBuilder.Build(SettingsNode<Grid>
        //     .New("default")
        //     .Argument("argument", String())
        //     .Group(SettingsGroup
        //         .New("group", ("s", String()))
        //         .Option(SettingsGroupOption
        //             .New("option1")
        //             .Argument("value", String())
        //             .Convert((cin, cout) => {
        //                 cout["s"].SetValue(cin["value"].GetValue());
        //             })
        //         )
        //         .Option(SettingsGroupOption
        //             .New("option2")
        //             .Argument("value", String())
        //             .Convert((cin, cout) => {
        //                 cout["s"].SetValue(cin["value"].GetValue());
        //             })
        //         )
        //     )
        // );
        //
        // Console.WriteLine(settingsBuilder.ToString());
        
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