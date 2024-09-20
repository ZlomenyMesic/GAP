//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Reflection;
using GAP.core.image.generation.generators;
using GAP.util.registries;
using Kolors;

namespace GAP.core.image.generation;

public abstract class ImageGeneratorRegistry : ClassRegistry<ImageGenerator> {

    internal static Type Register(string id, Type type) {
        return BaseRegister(id, type);
    }
    
    public static ImageGenerator GetInstance(string id, string jsonSettings) {
        Debug.info(REGISTRY.Count.ToString());
        
        if (REGISTRY.TryGetValue(id, out Type? value)) {
            if (value != null) {
                
                ImageGenerator? instance = (ImageGenerator?)Activator.CreateInstance(value);
                if (instance == null) {
                    throw new NullReferenceException($"Failed to create instance of type {value}");
                }
                
                MethodInfo? method = value.GetMethod("LoadFromJson");
                if (method == null) {
                    throw new NullReferenceException($"Can't find method LoadFromJson in type {value}"); 
                }
                
                instance.LoadFromJson(jsonSettings);
                return instance;
            }
            else throw new NullReferenceException(
                "Cannot create an instance of ImageGenerator. Requested type is null.");
        }
        else {
            throw new KeyNotFoundException($"Could not find registry object \'{id}\'.");
        }
    }

    static ImageGeneratorRegistry() {
        WHITE_NOISE = Register("gap:white_noise", typeof(WhiteNoise));
    }

    public static readonly Type WHITE_NOISE;
}