//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.core.image.generation;
using GAP.core.image.generation.generators;
using GAP.core.image.transformation;
using GAP.core.image.transformation.transformers;

namespace GAP;

/// <summary>
/// registers default generators and transformers
/// </summary>
public static class DefaultRegistries {
    
    public static void Register() {
        ImageGeneratorDispatcher igDispatcher = new ImageGeneratorDispatcher(GAP.PROJECT_ID);
        igDispatcher.Register("white_noise", typeof(WhiteNoise));
        
        ImageTransformerDispatcher itDispatcher = new ImageTransformerDispatcher(GAP.PROJECT_ID);
        itDispatcher.Register("grid", typeof(Grid));
        itDispatcher.Register("pixelize", typeof(Pixelize));
        itDispatcher.Register("rescale", typeof(Rescale));
    }
}