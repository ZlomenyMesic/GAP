namespace GAP.core.image;

public class ImageGeneratorDispatcher {
    public void RegisterImageGenerator(string id, Type type) {
        ImageGeneratorRegistry.Register(id, type);
    }
}