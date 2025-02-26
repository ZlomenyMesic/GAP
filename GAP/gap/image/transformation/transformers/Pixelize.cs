//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

using System.Drawing;
using System.Text.Json;
using GapCore;
using NeoKolors.Settings;

namespace GAP.gap.image.transformation.transformers;

/// <summary>
/// Pixelize Image Transform <br/>
/// creates a poor pixel effect over the image
/// </summary>
public class Pixelize : IImageTransformer<Pixelize>, ICloneable {
    public PixelType PixelType { get; set; }
    
    /// <summary>
    /// empty constructor
    /// </summary>
    public Pixelize() { }
    
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="pixelType">type of pixel color arrangement</param>
    public Pixelize(PixelType pixelType) {
        this.PixelType = pixelType;
    }
    
    /// <summary>
    /// main transform method
    /// </summary>
    /// <param name="image">input image</param>
    /// <returns>transformed image as <see cref="Bitmap"/></returns>
    public Bitmap TransformImage(Bitmap image) {
        if (PixelType == PixelType.STRIPES) {
            return PixelizeStripes(image);
        }
        else {
            return PixelizeSquares(image);
        }
    }

    public ISettingsBuilder<Pixelize> GetSettings() {
        return (ISettingsBuilder<Pixelize>)SETTINGS.Clone();
    }

    ISettingsBuilder<IImageTransformer> IImageTransformer.GetSettings() {
        return GetSettings();
    }

    private static Bitmap PixelizeStripes(Bitmap image) {
        Bitmap result = new Bitmap(image.Width * 3, image.Height * 3);
            
        for (int x = 0; x < image.Width; x++) {
            for (int y = 0; y < image.Height; y++) {
                result.SetPixel(x * 3, y * 3, Color.FromArgb(255, image.GetPixel(x, y).R, 0, 0));
                result.SetPixel(x * 3, y * 3 + 1, Color.FromArgb(255, image.GetPixel(x, y).R, 0, 0));
                result.SetPixel(x * 3, y * 3 + 2, Color.FromArgb(255, image.GetPixel(x, y).R, 0, 0));
                result.SetPixel(x * 3 + 1, y * 3, Color.FromArgb(255, 0, image.GetPixel(x, y).G, 0));
                result.SetPixel(x * 3 + 1, y * 3 + 1, Color.FromArgb(255, 0, image.GetPixel(x, y).G, 0));
                result.SetPixel(x * 3 + 1, y * 3 + 2, Color.FromArgb(255, 0, image.GetPixel(x, y).G, 0));
                result.SetPixel(x * 3 + 2, y * 3, Color.FromArgb(255, 0, 0, image.GetPixel(x, y).B));
                result.SetPixel(x * 3 + 2, y * 3 + 1, Color.FromArgb(255, 0, 0, image.GetPixel(x, y).B));
                result.SetPixel(x * 3 + 2, y * 3 + 2, Color.FromArgb(255, 0, 0, image.GetPixel(x, y).B));
            }
        }
            
        return result;
    }

    private static Bitmap PixelizeSquares(Bitmap image) {
        Bitmap result = new Bitmap(image.Width * 2, image.Height * 2);
        
        for (int x = 0; x < image.Width; x++) {
            for (int y = 0; y < image.Height; y++) {
                result.SetPixel(x * 2, y * 2, Color.FromArgb(255, image.GetPixel(x, y).R, 0, 0));
                result.SetPixel(x * 2, y * 2 + 1, Color.FromArgb(255, 0, image.GetPixel(x, y).G * 1/2, 0));
                result.SetPixel(x * 2 + 1, y * 2, Color.FromArgb(255, 0, image.GetPixel(x, y).G * 1/2, 0));
                result.SetPixel(x * 2 + 1, y * 2 + 1, Color.FromArgb(255, 0, 0, image.GetPixel(x, y).B));
            }
        }
        
        return result;
    }
    
    /// <summary>
    /// copies all fields into itself from another instance
    /// </summary>
    /// <param name="pixelize">other instance</param>
    private void Copy(Pixelize pixelize) {
        PixelType = pixelize.PixelType;
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
    public void LoadFromJson(string settings) {
        Pixelize? pixelize = JsonSerializer.Deserialize<Pixelize>(settings) ?? null;
        
        if (pixelize == null) {
            throw new JsonException("Deserialization of settings of Grid failed");
        }
        
        Copy(pixelize);
    }
    
    private static readonly ISettingsBuilder<Pixelize> SETTINGS = SettingsBuilder<Pixelize>.Build("pixelize", 
        SettingsNode<Pixelize>.New("default")
            .Argument("pixel_type", Arguments.SingleSelectEnum(PixelType.SQUARES))
            .Constructs(context => new Pixelize((PixelType)context["pixel_type"].Get()))
    );


    public override string ToString() => JsonSerializer.Serialize(this);
    
    public object Clone() {
        return MemberwiseClone();
    }
}

public enum PixelType {
    STRIPES,
    SQUARES
}