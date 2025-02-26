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

public class Rescale : IImageTransformer<Rescale>, ICloneable {

    private int Scale { get; set; }
    
    public Bitmap TransformImage(Bitmap image) {
        throw new NotImplementedException();
    }

    public ISettingsBuilder<Rescale> GetSettings() {
        throw new NotImplementedException();
    }

    ISettingsBuilder<IImageTransformer> IImageTransformer.GetSettings() {
        return GetSettings();
    }


    public override string ToString() => JsonSerializer.Serialize(this);
    
    public object Clone() {
        throw new NotImplementedException();
    }
}