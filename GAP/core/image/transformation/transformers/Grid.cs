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
public class Grid : ImageTransformer {
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
    public override Bitmap TransformImage(Bitmap image) {

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

    /// <summary>
    /// copies all fields into itself from another instance
    /// </summary>
    /// <param name="grid">other instance</param>
    private void Copy(Grid grid) {
        scaleFactor = grid.scaleFactor;
    }

    /// <summary>
    /// loads settings from a json string into an instance that called this method
    /// </summary>
    /// <param name="settings">json string</param>
    /// <exception cref="JsonException">
    /// if inputted json is invalid or if value returned by the
    /// <see cref="JsonSerializer.Deserialize{TValue}(System.IO.Stream,System.Text.Json.JsonSerializerOptions?)"/>
    /// returns null
    /// </exception>
    /// <exception cref="ArgumentException">if <see cref="settings"/> is null</exception>
    /// <exception cref="NotSupportedException">if no deserializer is available</exception>
    public override void LoadFromJson(string settings) {
        Grid? grid = JsonSerializer.Deserialize<Grid>(settings) ?? null;
        
        if (grid == null) {
            throw new JsonException("Deserialization of settings of Grid failed");
        }
        
        Copy(grid);
    }
    
    
    private static readonly SettingsBuilder<Grid> SETTINGS = SettingsBuilder<Grid>.Build("grid", 
        SettingsNode<Grid>.New("default")
            .Argument("scale_factor", Arguments.UInteger(0, 100))
            .OnParse(context => new Grid((uint)context["scale_factor"].GetParsedValue()))
    );
    

    public override SettingsBuilder<T> GetSettings<T>() {
        if (typeof(T) != typeof(Grid)) {
            throw new SettingsBuilderException("Invalid type inputted.");
        }
        
        return SETTINGS.Clone() as SettingsBuilder<T> ?? SettingsBuilder<T>.Empty<T>("white_noise");
    }

    public override string ToString() => JsonSerializer.Serialize(this);
}