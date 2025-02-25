//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

using System.Drawing;
using NeoKolors.Settings;

namespace GapCore;

/// <summary>
/// Image Transformer Interface <br/>
/// all image transformer classes must implement this class in order to be properly registered
/// </summary>
public interface IImageTransformer<TSelf> : IImageTransformer where TSelf : class, IImageTransformer<TSelf> {

    /// <summary>
    /// returns copy of settings available for the transformer
    /// </summary>
    public new ISettingsBuilder<TSelf> GetSettings();
}

public interface IImageTransformer {
    
    /// <summary>
    /// main transformation method
    /// </summary>
    /// <returns><see cref="Bitmap"/> object with the final image</returns>
    public Bitmap TransformImage(Bitmap image);
    
    /// <summary>
    /// returns copy of settings available for the transformer
    /// </summary>
    public ISettingsBuilder<IImageTransformer> GetSettings();
}