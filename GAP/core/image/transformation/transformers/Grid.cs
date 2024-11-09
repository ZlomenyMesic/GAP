//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using System.Text.Json;
using GAP.util.exceptions;
using GAP.util.settings;

namespace GAP.core.image.transformation.transformers;

/// <summary>
/// Grid Image Transformation <br/>
/// transforms the picture by separating its pixels and placing transparent color between them 
/// </summary>
public class Grid : IImageTransformer, ICloneable {
    public uint scaleFactor { get; set; }
    
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="scaleFactor"></param>
    /// <exception cref="ArgumentException"></exception>
    public Grid(uint scaleFactor) {

        if (scaleFactor == 0) throw new ArgumentException("Scale factor for Grid transform must be greater than zero");

        this.scaleFactor = scaleFactor;
    }
    
    /// <summary>
    /// empty constructor
    /// </summary>
    public Grid() { }
    
    /// <summary>
    /// main transform method
    /// </summary>
    /// <param name="image">input image</param>
    /// <returns>transformed image as <see cref="Bitmap"/></returns>
    public Bitmap TransformImage(Bitmap image) {

        if (scaleFactor == 1) return image;

        int width = (int)(image.Width * scaleFactor);
        int height = (int)(image.Height * scaleFactor);
        Bitmap result = new Bitmap(width, height);

        for (int x = 0; x < image.Width; x++) {
            for (int y = 0; y < image.Height; y++) {
                result.SetPixel((int)(x * scaleFactor), (int)(y * scaleFactor), image.GetPixel(x, y));
            }
        }
            
        return result;
    }
    
    private static readonly ISettingsBuilder<Grid, Grid> SETTINGS = SettingsBuilder<Grid>.Build("grid", 
        SettingsNode<Grid>.New("default")
            .Argument("scale_factor", Arguments.UInteger(0, 100))
            .OnParse(context => new Grid((uint)context["scale_factor"].GetParsedValue()))
    );

    public static SettingsBuilder<Grid> GetSettings() {
        return (SettingsBuilder<Grid>)SETTINGS.Clone();
    }

    public override string ToString() => JsonSerializer.Serialize(this);
    
    public object Clone() {
        return MemberwiseClone();
    }
}