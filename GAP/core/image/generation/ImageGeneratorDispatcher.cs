//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

namespace GAP.core.image.generation;

public class ImageGeneratorDispatcher {
    public void RegisterImageGenerator(string id, Type type) {
        ImageGeneratorRegistry.Register(id, type);
    }
}