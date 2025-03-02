﻿//
// GAP
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 
// founded 11.9.2024
//

using System.Drawing;
using System.Drawing.Imaging;
using GAP.gap.image.transformation.transformers;
using GapCore;
using GapCore.modLoader;
using NeoKolors.Console;

namespace GAP;

// ReSharper disable once InconsistentNaming
[AutomaticallyLoaded]
internal class GAP : IMod {

    // constants
    public const string PROJECT_ID = "gap";
    private const string DESCRIPTION = "official vanilla GAP generators";
    private const Debug.DebugLevel DEBUG_LEVEL = Debug.DebugLevel.ALL;
    private static string MOD_DIRECTORY = ".";
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
    
    private const string GAP_ASCII_ART = 
        " g_____   a___  p______            g_____                           _   _              a___       _    p____" +
        "__                                    \ng|  __ \\ a/ _ \\ p| ___ \\          g|  __ \\                      " +
        "   | | (_)            a/ _ \\     | |   p| ___ \\                                   \ng| |  \\// a/_\\ \\p| " +
        "|_/ /  d______  g| |  \\/ ___ _ __   ___ _ __ __ _| |_ ___   _____  a/ /_\\ \\_ __| |_  p| |_/ / __ ___   __" +
        " _ _ __ __ _ _ __ ___  \ng| | __ a|  _  |p|  __/  d|______| g| | __ / _ \\ '_ \\ / _ \\ '__/ _` | __| \\ \\ " +
        "/ / _ \\ a|  _  | '__| __| p|  __/ '__/ _ \\ / _` | '__/ _` | '_ ` _ \\ \ng| |_\\ \\a| | | |p| |            " +
        "  g| |_\\ \\  __/ | | |  __/ | | (_| | |_| |\\ V /  __/ a| | | | |  | |_  p| |  | | | (_) | (_| | | | (_| | " +
        "| | | | |\n g\\____/a\\_| |_/p\\_|               g\\____/\\___|_| |_|\\___|_|  \\__,_|\\__|_| \\_/ \\___| a" +
        "\\_| |_/_|   \\__| p\\_|  |_|  \\___/ \\__, |_|  \\__,_|_| |_| |_|\n                                        " +
        "                                                                           __/ |                    \n      " +
        "                                                                                                            " +
        "|___/                     ";

    private const string GAP_ASCII_ART_2 =
        "  g*________    a*_____ p*__________ \n g*/  _____/   a*/  _  \\p*\\______   \\\ng*/   \\  ___  a*/  /_\\  \\p*|     " +
        "___/\ng*\\    \\_\\  \\a*/    |    \\    p*|    \n g*\\______  /a*\\____|__  /p*____|    \n        g*\\/         a*" +
        "\\/ d*generative art producer\n\n";
    
    private const int G_GREEN = 0x8AC926;
    private const int A_BLUE = 0x1982C4;
    private const int P_PURPLE = 0x6A4C93;
    private const int D_RED = 0xFF595E;
    private const int D_YELLOW = 0xFFCA3A;

    //
    // NOTE:
    // to count lines in project call " git ls-files | xargs wc -l " in a linux shell
    // for a specific file type " git ls-files | grep '\.<file-extension>' | xargs wc -l "
    //
    
    //
    // TODOS:
    //
    //

    // public static event EventHandler Event;
    
    static int Main(string[] args) {

        MOD_DIRECTORY = args.Length > 0 ? args[0] : ".";
        
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
        Console.WriteLine();
        Console.WindowWidth = 140;
        ConsoleColors.PrintlnComplexColored(GAP_ASCII_ART_2, ("g*", G_GREEN), ("a*", A_BLUE), ("p*", P_PURPLE), ("d*", 0x919397));
        
        // Debug levels (maybe change this to no info later?)
        Debug.Level = DEBUG_LEVEL;

        Debug.InfoColor = 0x85c46c;
        Debug.WarnColor = 0xd9b72b;
        Debug.ErrorColor = 0xff5647;
        
        OperatingSystem os = Environment.OSVersion;
        Debug.Info($"Platform: {os}");
        
        // Mod loading
        Debug.Info($"Starting mod loading from directory '{Path.GetFullPath(MOD_DIRECTORY)}'\n");
        ModLoader.LoadMods(MOD_DIRECTORY, EXCLUDED_BINARIES);
        ModLoader.WriteRegisteredMods();
        
        // --- END OF MAIN FUNCTIONALITY ---
        // you can put your code here:

        foreach (var x in ImageGeneratorRegistry.GetAll()) {
            Console.WriteLine(x.ToString());
        }
        
        // for (int i = 0; i < 100; i++) {
        //     var s = new Stripes(30, 30, 10, 5, i);
        //     s.GenerateImage().Save($@".\gallery\curly_lines-1920x1080\{SeedFormat.WordFromSeed(i)}.png");
        // }

        var v = Image.FromFile(@"C:\Users\krystof\Desktop\peng.png");
        var c = new Bitmap(v);

        // Grid g = new Grid(2);
        // g.TransformImage(c).Save("./gridizes.png", ImageFormat.Png);

        var p = new Pixelize(PixelType.SQUARES);
        p.TransformImage(c).Save("./pixels.png", ImageFormat.Png);
        
        // DeepDream.RunGenerator();
        // Ladybug.TrainModel();
        // Ladybug.RunGenerator();
        Console.WriteLine("Hello, World!");

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