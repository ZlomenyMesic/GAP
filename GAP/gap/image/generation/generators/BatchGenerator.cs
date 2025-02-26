//
// GAP
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
//

using System.Drawing;
using System.Drawing.Imaging;
using GapCore;
using GapCore.util;
using NeoKolors.Settings;

namespace GAP.gap.image.generation.generators;

public class BatchGenerator : IImageGenerator<BatchGenerator> {
    
    // TODO finish batch generator

    public IBatchableGenerator? Generator { get; private set; }
    public int Iterations { get; private set; }
    public int StartSeed { get; private set; }
    public NameType NameType { get; private set; }
    

    public BatchGenerator(IBatchableGenerator? generator, int iterations, int startSeed, NameType nameType) {
        Generator = generator;
        Iterations = iterations;
        StartSeed = startSeed;
        NameType = nameType;
    }
    
    public BatchGenerator() { }
    
    /// <summary>
    /// returns the first generated image and creates a batch of images outputted from a generator
    /// </summary>
    public Bitmap GenerateImage() {
        
        if (Generator == null) {
            throw new NullReferenceException("Generator is null.");
        }
        
        for (int i = StartSeed; i < Iterations + StartSeed; i++) {
            Bitmap bmp = Generator.GenerateImage();
            bmp.Save($"./gallery/{SeedFormat.WordFromSeed(i)}.png", ImageFormat.Png); 
        }
        
        return Generator.GenerateImage();
    }

    ISettingsBuilder<IImageGenerator> IImageGenerator.GetSettings() => GetSettings();

    public SettingsBuilder<BatchGenerator> GetSettings() {
        throw new NotImplementedException();
    }
}

public enum NameType {
    NUMBER,
    WORD
}