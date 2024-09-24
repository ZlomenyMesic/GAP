//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Reflection;
using GAP.core.image.transformation.transformers;
using GAP.util.registries;
using Kolors;

namespace GAP.core.image.transformation;

public class ImageTransformerRegistry : ClassRegistry<ImageTransformer> {
   
    internal static Type Register(string id, Type type) {
        return BaseRegister(id, type);
    }
    
    public static ImageTransformer GetInstance(string id, string jsonSettings) {
        Debug.info(REGISTRY.Count.ToString());
        
        if (REGISTRY.TryGetValue(id, out Type? value)) {
            if (value != null) {
                
                ImageTransformer? instance = (ImageTransformer?)Activator.CreateInstance(value);
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
                "Cannot create an instance of ImageTransformer. Requested type is null.");
        }
        else {
            throw new KeyNotFoundException($"Could not find registry object \'{id}\'.");
        }
    }
}