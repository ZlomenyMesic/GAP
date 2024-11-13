using System.Drawing;
using System.Drawing.Imaging;
using GAP.util;

namespace GAP.core.image.generation.generators;

public class BatchGenerator : IImageGenerator {
    
    // TODO finish batch generator
    
    private IBatchableGenerator generator { get; set; }
    private int iterations { get; set; }
    private int startSeed { get; set; }
    private NameType nameType { get; set; }
    
    public IBatchableGenerator Generator => generator;
    public int Iterations => iterations;
    public int StartSeed => startSeed;
    public NameType NameType => nameType;

    public BatchGenerator(IBatchableGenerator generator, int iterations, int startSeed, NameType nameType) {
        this.generator = generator;
        this.iterations = iterations;
        this.startSeed = startSeed;
        this.nameType = nameType;
    }
    
    /// <summary>
    /// returns the first generated image and creates a batch of images outputted from a generator
    /// </summary>
    public Bitmap GenerateImage() {
        
        for (int i = startSeed; i < iterations + startSeed; i++) {
            Bitmap bmp = generator.GenerateImage();
            bmp.Save($"./gallery/{SeedFormat.WordFromSeed(i)}.png", ImageFormat.Png); 
        }
        
        return generator.GenerateImage();
    }

    public static object GetSettings() {
        throw new NotImplementedException();
    }
}

public enum NameType {
    NUMBER,
    WORD
}