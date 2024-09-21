//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using System.Text.Json;

namespace GAP.core.image.transformation;

public abstract class ImageTransformer {
    public abstract int seed { get; set; }

    public ImageTransformer() {}
    public abstract Bitmap TransformImage(Bitmap image);
    public abstract void LoadFromJson(string settings);
    public override string ToString() => JsonSerializer.Serialize(this);
}