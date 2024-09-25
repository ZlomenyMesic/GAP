using GAP.core.image.generation;
using GAP.core.image.generation.generators;
using GAP.core.image.transformation;
using GAP.core.image.transformation.transformers;

namespace GAP;

public static class DefaultRegistries {
    
    public static void Register() {
        ImageGeneratorDispatcher igDispatcher = new ImageGeneratorDispatcher(Program.PROJECT_ID);
        igDispatcher.Register("white_noise", typeof(WhiteNoise));
        
        ImageTransformerDispatcher itDispatcher = new ImageTransformerDispatcher(Program.PROJECT_ID);
        itDispatcher.Register("grid", typeof(Grid));
        itDispatcher.Register("pixelize", typeof(Pixelize));
        itDispatcher.Register("rescale", typeof(Rescale));
    }
}