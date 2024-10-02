//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Reflection;
using Kolors;

namespace GAP.core.modLoader;

/// <summary>
/// Mod Loader <br/>
/// loads available mods / extensions from a directory
/// </summary>
public class ModLoader {
    
    private static List<string> REGISTERED_MOD_IDS { get; } = [];
    
    /// <summary>
    /// loads all available mods from a specified directory
    /// </summary>
    /// <param name="path">directory path</param>
    /// <returns>number of mods loaded</returns>
    public static int LoadMods(string path) {
        int modCount = 0;
        
        foreach (var file in Directory.GetFiles(path, "*.dll")) {
            var assembly = Assembly.LoadFrom(file);
            
            bool result = RegisterMod(assembly);
            
            if (result) modCount++;
        }
        
        return modCount;
    }

    /// <summary>
    /// registers a single mod
    /// </summary>
    /// <param name="modAssembly">assembly file of the extension</param>
    /// <returns>whether the mod was successfully loaded</returns>
    private static bool RegisterMod(Assembly modAssembly) {
        foreach (var type in modAssembly.GetTypes()) {
            if (typeof(Mod).IsAssignableFrom(type) && !type.IsInterface) {
                
                var mod = (Mod?)Activator.CreateInstance(type);
                
                if (mod == null) {
                    Debug.warn($"Mod loader failed to load type {type.FullName} from assembly {modAssembly.FullName}");
                    continue;
                }
                
                string modId = mod.Register();

                if (REGISTERED_MOD_IDS.Contains(modId)) {
                    Debug.warn($"A mod with the same id ('{modId}') has already been registered!");
                    continue;
                }
                
                // main mod initialization
                mod.Initialize();
                
                REGISTERED_MOD_IDS.Add(modId);
                return true;
            }
        }
        
        return false;
    }

    /// <summary>
    /// returns ids of all registered mods
    /// </summary>
    public static string[] GetRegisteredMods() {
        return REGISTERED_MOD_IDS.ToArray();
    }
}