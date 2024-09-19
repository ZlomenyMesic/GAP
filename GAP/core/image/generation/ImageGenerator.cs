//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;

namespace GAP.core.image.generation;

public abstract class ImageGenerator {
    public abstract int width { get; protected set; }
    public abstract int height { get; protected set; }
    public abstract int seed { get; protected set; }

    
    public abstract Bitmap GenerateImage();
    public abstract void LoadFromJson(string settings);
}