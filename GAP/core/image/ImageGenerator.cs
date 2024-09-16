//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using GAP.core.input;
using GAP.util;

namespace GAP.core.image;

public abstract class ImageGenerator {
    public abstract int width { get; }
    public abstract int height { get; }
    public abstract int seed { get; }

    // public abstract SettingsBuilder GetSettings();
    public abstract ImageGenerator GetInstance(int width, int height, int seed/*, Settings settings*/);
    public abstract Bitmap GenerateImage();
}