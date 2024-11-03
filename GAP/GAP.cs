//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

using GAP.core.image.generation.generators;
using GAP.core.modLoader;
using Kolors;
using GAP.machineLearning.deepdream;
using GAP.util.settings;
using NAudio.Midi;

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
        "NAudio.WinMM",
        "Microsoft.CodeAnalysis.dll"
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
        Debug.Level = DEBUG_LEVEL;

        Debug.InfoColor = 0x85c46c;
        Debug.WarnColor = 0xd9b72b;
        Debug.ErrorColor = 0xff5647;
        
        // Mod loading
        ModLoader.LoadMods(".", EXCLUDED_BINARIES);
        ModLoader.WriteRegisteredMods();

        // --- END OF MAIN FUNCTIONALITY ---
        // you can put your code here:

        MidiEventCollection mec = new MidiEventCollection(1, 960);
        mec.AddTrack([new TempoEvent(600000, 0), new MetaEvent(MetaEventType.EndTrack, 0, 0)]);
        mec.AddTrack([new PatchChangeEvent(0, 2, 1), new NoteOnEvent(0, 2, 48, 32, 980), new NoteEvent(980, 2, MidiCommandCode.NoteOff, 48, 0), new MetaEvent(MetaEventType.EndTrack, 0, 1000)]);
        
        MidiFile.Export("./idk.midi", mec);

        var b = WhiteNoise.GetSettings<WhiteNoise>();
        b["basic"].SetValue("width", 1024);
        b["basic"].SetValue("height", 768);
        b["basic"].SetGroupOptionValue("seed", "word", "seed", "adamati");

        WhiteNoise w = b.Execute("basic", ("seed", "word"));
        
        Console.WriteLine(w.ToString());
        
        // DeepDream.Iterations = 5;
        // DeepDream.Octaves = 8;
        // DeepDream.LayerActivationFunction = DeepDream.LayerActivationFunctions.LastPriority;
        // DeepDream.LoadParametersFile();
        // DeepDream.RunGeneratorFilteredRandom(3);

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