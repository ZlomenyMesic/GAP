using System.Drawing;
using GAP.util.settings;
using Kolors;

namespace GAP.core.image.transformation.transformers;

public class ColorReduce : IImageTransformer {
    
    /// <summary>
    /// value between 0 - 128, how much of the bits of color channel is lost,
    /// 0 -> none, 128 -> all except the most significant
    /// </summary>
    private int reduceLevel { get; set; }
    public int ReduceLevel => reduceLevel;

    public ColorReduce(int reduceLevel) {
        this.reduceLevel = Math.Max(Math.Min(reduceLevel, 128), 0);
    }
    
    public Bitmap TransformImage(Bitmap image) {
        for (int x = 0; x < image.Width; x++) {
            for (int y = 0; y < image.Height; y++) {
                Color c = image.GetPixel(x, y);
                (int r, int g, int b) = (c.R, c.G, c.B);
                r &= ~reduceLevel;
                g &= ~reduceLevel;
                b &= ~reduceLevel;
                
                image.SetPixel(x, y, Color.FromArgb(r, g, b));
            }
        }
        
        return image;
    }


    private static readonly ISettingsBuilder<ColorReduce, ColorReduce> SETTINGS = SettingsBuilder<ColorReduce>.Build("color_reduce", 
        SettingsNode<ColorReduce>.New("color_reduce")
            .Argument("reduce_level", Arguments.Integer(0, 128))
            .OnParse(cin => new ColorReduce((int)cin["reduce_level"].GetParsedValue()))
    );

    public static ISettingsBuilder<ColorReduce, ColorReduce> GetSettings() {
        return SETTINGS.Clone();
    }
}