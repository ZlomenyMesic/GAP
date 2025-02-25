//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

using System.Drawing;
using GAP.util;
using GapCore.util;
using GapCore.util.math;
using NeoKolors.Common;
using NeoKolors.Settings;
using Color = System.Drawing.Color;

namespace GapCore;

/// <summary>
/// Image Generator Interface <br/>
/// all image generator classes must implement this class in order to be properly registered
/// </summary>
public interface IImageGenerator<TSelf> : IImageGenerator where TSelf : class, IImageGenerator<TSelf> {
    
    /// <summary>
    /// returns copy of settings available for the generator
    /// </summary>
    public new SettingsBuilder<TSelf> GetSettings();
}

public interface IImageGenerator {
    /// <summary>
    /// main generation method
    /// </summary>
    /// <returns><see cref="Bitmap"/> object with the final image</returns>
    public Bitmap GenerateImage();
    
    private static readonly SettingsGroup UNIVERSAL_SEED_SETTINGS = SettingsGroup
        .New("seed", Context.New(("seed", Arguments.Integer())))
        .Option(SettingsGroupOption
            .New("number")
            .Argument("seed", Arguments.Integer())
            .EnableAutoMerge()
        )
        .Option(SettingsGroupOption
            .New("word")
            .Argument("seed", Arguments.String())
            .Merges((cin, cout) => {
                cout["seed"] <<= SeedFormat.SeedFromWord((string)cin["seed"].Get());
            })
        )
        .Option(SettingsGroupOption
            .New("string")
            .Argument("seed", Arguments.String())
            .Merges((cin, cout) => {
                cout["seed"] <<= Hash.GetHashCode((string)cin["seed"].Get());
            })
        )
        .EnableAutoMerge();
        
    /// <summary>
    /// returns a clone of the universal seed input group
    /// </summary>
    public static SettingsGroup UniversalSeedInput() {
        return UNIVERSAL_SEED_SETTINGS;
    }

    /// <summary>
    /// returns the darkest color of the palette
    /// </summary>
    public static sealed Color GetDarkest(ColorPalette palette) {
        double minV = int.MaxValue;
        int minI = 0;
        
        for (int i = 0; i < palette.Colors.Length; i++) {
            (_, _, double v) = ColorFormat.ColorToHsv(Color.FromArgb(palette.Colors[i]));

            if (!(v < minV)) continue;
            
            minV = v;
            minI = i;
        }
        
        // choose a background color from the palette
        int background = palette.Colors[minI] + (0xff << 24);
        
        return Color.FromArgb(background);
    }

    public ISettingsBuilder<IImageGenerator> GetSettings();
}