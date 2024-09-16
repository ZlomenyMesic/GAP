using GAP.core.image.generators;
using GAP.core.input;
using GAP.util.registries;
using Kolors;

namespace GAP.core.image;

public abstract class ImageGeneratorRegistry : ClassRegistry<ImageGenerator> {

    internal static Type Register(string id, Type type) {
        return BaseRegister(id, type);
    }
    
    public static ImageGenerator GetInstance(string id, int width, int height, int seed/*, Settings settings*/) {
        ImageGenerator instance;
        
        Debug.info(REGISTRY.Count.ToString());
        
        if (REGISTRY.TryGetValue(id, out Type? value)) {
            if (value != null) 
                instance = (ImageGenerator)Activator.CreateInstance(value, width, height, seed/*, settings*/)!;
            else throw new NullReferenceException(
                "Cannot create an instance of ImageGenerator. Requested type is null.");
        }
        else {
            // Debug.error($"Could not find registry object \'{id}\'.");
            throw new KeyNotFoundException($"Could not find registry object \'{id}\'.");
        }
        
        return instance;
    }
    
    public static readonly Type WHITE_NOISE = Register("gap:white_noise", typeof(WhiteNoise));
}