//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Reflection;
using NeoKolors.Console;

namespace GAP.core.modLoader;

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

        bool baseLoaded = false;
        foreach (var i in MOD_IDS) {
            if (i == "gap") {
                baseLoaded = true;
                break;
            }
        }

        if (!baseLoaded) {
            GAP g = new GAP();
            
            MOD_IDS.Add(g.Register());
            MOD_DESCRIPTIONS.Add(g.GetInfo());
            g.Initialize();
        }
        
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
                
                if (mod == null) {
                    Debug.Warn($"Mod loader failed to load type {type.FullName} from assembly {modAssembly.FullName}");
                    continue;
                }
                
                string modId = mod.Register();

                if (MOD_IDS.Contains(modId)) {
                    Debug.Warn($"A mod with the same id ('{modId}') has already been registered!");
                    continue;
                }
                
                // main mod initialization
                mod.Initialize();
                
                MOD_IDS.Add(modId);
                MOD_DESCRIPTIONS.Add(mod.GetInfo());
                return true;
            }
        }
        
        return false;
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