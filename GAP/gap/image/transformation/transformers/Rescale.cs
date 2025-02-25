//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using System.Text.Json;
using NeoKolors.Settings;

namespace GAP.core.image.transformation.transformers;

public class Rescale : IImageTransformer<Rescale>, ICloneable {

    private int Scale { get; set; }
    
    public Bitmap TransformImage(Bitmap image) {
        throw new NotImplementedException();
    }

    public ISettingsBuilder<Rescale> GetSettings() {
        throw new NotImplementedException();
    }

    ISettingsBuilder<IImageTransformer> IImageTransformer.GetSettings() {
        return GetSettings();
    }


    public override string ToString() => JsonSerializer.Serialize(this);
    
    public object Clone() {
        throw new NotImplementedException();
    }
}