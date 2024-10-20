//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using System.Text.Json;
using GAP.util.settings;

namespace GAP.core.image.transformation.transformers;

public class Rescale : ImageTransformer {
    
    public override Bitmap TransformImage(Bitmap image) {
        throw new NotImplementedException();
    }

    public override void LoadFromJson(string settings) {
        throw new NotImplementedException();
    }

    public override SettingsBuilder<T> GetSettings<T>() {
        throw new NotImplementedException();
    }

    public override string ToString() => JsonSerializer.Serialize(this);
}