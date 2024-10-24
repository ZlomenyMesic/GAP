//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using System.Text.Json;
using GAP.util.settings;

namespace GAP.core.image.transformation.transformers;

public class Rescale : IImageTransformer, ICloneable {

    private int scale { get; set; }
    
    public Bitmap TransformImage(Bitmap image) {
        throw new NotImplementedException();
    }

    public SettingsBuilder<T> GetSettings<T>() {
        throw new NotImplementedException();
    }

    public override string ToString() => JsonSerializer.Serialize(this);
    
    public object Clone() {
        throw new NotImplementedException();
    }
}