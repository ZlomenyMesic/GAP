//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using System.Text.Json;
using NeoKolors.Settings;

namespace GAP.core.image.transformation.transformers;

public class Rescale : IImageTransformer, ICloneable {

    private int Scale { get; set; }
    
    public Bitmap TransformImage(Bitmap image) {
        throw new NotImplementedException();
    }

    public SettingsBuilder<Rescale> GetSettings() {
        throw new NotImplementedException();
    }

    public override string ToString() => JsonSerializer.Serialize(this);
    
    public object Clone() {
        throw new NotImplementedException();
    }
}