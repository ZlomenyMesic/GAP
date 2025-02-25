//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

using System.Drawing;
using NeoKolors.Settings;

namespace GAP.core.image.transformation.transformers;

public class ColorReduce : IImageTransformer<ColorReduce> {
    
    /// <summary>
    /// value between 0 - 128, how much of the bits of color channel is lost,
    /// 0 -> none, 128 -> all except the most significant
    /// </summary>
    public int ReduceLevel { get; private set; }

    public ColorReduce(int reduceLevel) {
        ReduceLevel = Math.Max(Math.Min(reduceLevel, 128), 0);
    }

    public ColorReduce() {}
    
    public Bitmap TransformImage(Bitmap image) {
        for (int x = 0; x < image.Width; x++) {
            for (int y = 0; y < image.Height; y++) {
                Color c = image.GetPixel(x, y);
                (int r, int g, int b) = (c.R, c.G, c.B);
                r &= ~ReduceLevel;
                g &= ~ReduceLevel;
                b &= ~ReduceLevel;
                
                image.SetPixel(x, y, Color.FromArgb(r, g, b));
            }
        }
        
        return image;
    }

    public ISettingsBuilder<ColorReduce> GetSettings() {
        return (ISettingsBuilder<ColorReduce>)SETTINGS.Clone();
    }

    ISettingsBuilder<IImageTransformer> IImageTransformer.GetSettings() {
        return GetSettings();
    }


    private static readonly ISettingsBuilder<ColorReduce> SETTINGS = SettingsBuilder<ColorReduce>.Build("color_reduce", 
        SettingsNode<ColorReduce>.New("color_reduce")
            .Argument("reduce_level", Arguments.Integer(0, 128))
            .Constructs(cin => new ColorReduce((int)cin["reduce_level"].Get()))
    );

}