//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using System.Reflection;

namespace GAP.core.image.generation;

public abstract class ImageGenerator {
    public abstract int width { get; set; }
    public abstract int height { get; set; }
    public abstract int seed { get; set; }

    public ImageGenerator() { }
    
    public abstract Bitmap GenerateImage();
    public abstract void LoadFromJson(string settings);
}