//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using System.Text.Json;

namespace GAP.core.image.transformation.transformers;

public class Rescale : ImageTransformer {
    public override int seed { get; set; }
    
    
    public override Bitmap TransformImage(Bitmap image) {
        throw new NotImplementedException();
    }

    public override void LoadFromJson(string settings) {
        throw new NotImplementedException();
    }
    
    public override string ToString() => JsonSerializer.Serialize(this);
}