//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

using System.Reflection;
using GapCore.util;
using NeoKolors.Console;

namespace GapCore.modLoader;

/// <summary>
/// Mod Loader <br/>
/// loads available mods / extensions from a directory
/// </summary>
internal static class ModLoader {
    
    private static readonly List<string> MOD_IDS = [];
    private static readonly List<string> MOD_DESCRIPTIONS = [];

    /// <summary>
    /// loads all available mods from a specified directory
    /// </summary>
    /// <param name="path">directory path</param>
    /// <param name="excluded"></param>
    /// <returns>number of mods loaded</returns>
    public static int LoadMods(string path, params string[] excluded) {
        int modCount = 0;
        
        Console.WriteLine("\x1B[1mFound Binaries:\x1B[0m");
        
        foreach (var file in Directory.GetFiles(path, "*.dll")) {

            if (excluded.Any(e => file == e) || excluded.Any(e => file == Path.Combine(path, e)) || 
                excluded.Any(e => file == e + ".dll") || excluded.Any(e => file == Path.Combine(path, e + ".dll"))) 
            {
                continue;
            }
            
            var assembly = Assembly.LoadFrom(file);
            
            bool result = RegisterMod(assembly);

            if (result) {
                modCount++;
                
                ConsoleColors.PrintColored("   \u25cf ", Debug.InfoColor);
                Console.WriteLine(file.Replace(path + "\\", ""));
            }
            else {
                ConsoleColors.PrintColored("   \u25cb ", Debug.ErrorColor);
                Console.WriteLine(file.Replace(path + "\\", ""));
            }
        }

        // bool baseLoaded = false;
        // foreach (var i in MOD_IDS) {
        //     if (i == "gap") {
        //         baseLoaded = true;
        //         break;
        //     }
        // }
        //
        // if (!baseLoaded) {
        //     GAP g = new GAP();
        //     
        //     MOD_IDS.Add(g.Register());
        //     MOD_DESCRIPTIONS.Add(g.GetInfo());
        //     g.Initialize();
        // }
        
        Console.WriteLine();
        return modCount;
    }

    /// <summary>
    /// registers a single mod
    /// </summary>
    /// <param name="modAssembly">assembly file of the extension</param>
    /// <returns>whether the mod was successfully loaded</returns>
    private static bool RegisterMod(Assembly modAssembly) {
        foreach (var type in modAssembly.GetTypes()) {
            if (typeof(IMod).IsAssignableFrom(type) && !type.IsInterface) {
                var mod = (IMod?)Activator.CreateInstance(type);
                
                if (InitializeMod(mod, type, modAssembly) == 1) {
                    continue;
                }
                
                return true;
            }
        }
        
        return false;
    }
    
    private static int InitializeMod(IMod? mod, Type type, Assembly modAssembly) {
        if (mod == null) { 
            Debug.Warn($"Mod loader failed to load type {type.FullName} from assembly {modAssembly.FullName}");
            return 1;
        }
                
        string modId = mod.Register();

        if (MOD_IDS.Contains(modId)) {
            Debug.Warn($"A mod with the same id ('{modId}') has already been registered!");
            return 1;
        }
                
        var modAttributes = mod.GetType().GetCustomAttributes();
        bool autoLoad = false;

        foreach (var a in modAttributes) {
            if (a.GetType() == typeof(AutomaticallyLoadedAttribute)) {
                autoLoad = ((AutomaticallyLoadedAttribute)a).LoadAutomatically;
                break;
            }
        }
                
        // main mod initialization
        if (!autoLoad) {
            mod.Initialize();
        }
        else {
            foreach (var t in modAssembly.GetTypes()) {
                
                // if t is one of the base interfaces, skip it
                if ((!t.IsAssignableTo(typeof(IImageGenerator)) || t == typeof(IImageGenerator) || t == typeof(IImageGenerator<>)) &&
                    (!t.IsAssignableTo(typeof(IImageTransformer)) || t == typeof(IImageTransformer) || t == typeof(IImageTransformer<>)) ||
                    t == typeof(IBatchableGenerator) ||
                    t == typeof(IBatchableGenerator<>)) 
                {
                    continue;
                }

                // check if the class is excluded from mod loading
                var typeAttributes = t.GetCustomAttributes();
                bool exclude = false;
                    
                foreach (var a in typeAttributes) {
                    if (a.GetType() == typeof(ExcludeFromModLoadingAttribute) && ((ExcludeFromModLoadingAttribute)a).Exclude) {
                        exclude = true;
                        break;
                    }
                }

                if (exclude) continue;
                    
                if (t.IsAssignableTo(typeof(IImageGenerator))) {
                    ImageGeneratorRegistry.Register($"{mod.Register()}:{NameConverter.CodeNameToId(t.Name)}", t);
                }
                        
                else if (t.IsAssignableTo(typeof(IImageTransformer))) {
                    ImageTransformerRegistry.Register($"{mod.Register()}:{NameConverter.CodeNameToId(t.Name)}", t);
                }
            }
        }
                
        MOD_IDS.Add(modId);
        MOD_DESCRIPTIONS.Add(mod.GetInfo());
        return 0;
    }

    /// <summary>
    /// returns ids of all registered mods
    /// </summary>
    public static string[] GetRegisteredMods() {
        return MOD_IDS.ToArray();
    }

    /// <summary>
    /// writes the registered mods to console
    /// </summary>
    public static void WriteRegisteredMods() {
        Console.WriteLine($"\x1B[1mRegistered Mods ({MOD_IDS.Count}):\x1B[0m");
        
        for (int i = 0; i < MOD_IDS.Count; i++) {
            Console.WriteLine($"   \x1B[1m{MOD_IDS[i]}\x1b[0m: {MOD_DESCRIPTIONS[i]}");
        }
        
        Console.WriteLine();
    }
}