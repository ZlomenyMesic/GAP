//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using GAP.util.settings;
using Kolors;

namespace GAP.core.image.generation.generators;

public class Spectrogram : IImageGenerator, IBatchableGenerator {
    private int width { get; set; }
    private int height { get; set; }
    private int seed { get; set; }
    
    public int Width => width;
    public int Height => height;
    public int Seed => seed;

    private int lineLifetime { get; set; }
    private int step { get; set; }
    
    public int LineLifetime => lineLifetime;
    public int Step => step;

    /// <summary>
    /// 2d spectrogram line image generator
    /// </summary>
    /// <param name="width">image width</param>
    /// <param name="height">image height</param>
    /// <param name="seed">seed</param>
    /// <param name="lineLifetime">how long will the line be</param>
    /// <param name="step">idk what that is</param>
    public Spectrogram(int width, int height, int seed, int lineLifetime = 15000, int step = 200) {
        this.width = width;
        this.height = height;
        this.seed = seed;
        this.lineLifetime = lineLifetime;
        this.step = step;
    }
    
    public Bitmap GenerateImage() {
        Bitmap bmp = new Bitmap(width, height);
        Random rnd = new Random(seed);

        float xFrequency = rnd.NextSingle();
        float yFrequency = rnd.NextSingle();
        int xOffset = width / 2;
        int yOffset = height / 2; 
        float factor = width > height ? (float)width * 1/3 : (float)height * 1/3;
        float startOffset = rnd.NextSingle();
        Func<float, float> xFunc = GetFunc();
        Func<float, float> yFunc = GetFunc();
        
        var A = (rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle());
        var B = (rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle());
        var C = (rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle());
        var D = (rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle());
        
        Color background = Color.FromArgb(255, 16, 16 ,16);
        Graphics g = Graphics.FromImage(bmp);
        g.Clear(background);
        
        for (float j = startOffset; j < lineLifetime / (float)step + startOffset; j += 1 / (float)step) {
            bmp.SetPixel((int)(xOffset + xFunc(xFrequency * j) * factor), 
                (int)(yOffset + yFunc(yFrequency * j) * factor), ColorPalette.GenerateColorAtX(A, B, C, D, j));
        }
        
        return bmp;
    }

    private Func<float, float> GetFunc() {
        Random rnd = new Random(seed);
        
        return rnd.Next(1, 7) switch {
            1 => Sin,
            2 => Cos,
            3 => val => Sigmoid(NestedSin(val)),
            4 => val => Sigmoid(NestedCos(val)),
            5 => val => Sigmoid(SinCos2(val)),
            6 => val => Sigmoid(CosSin2(val)),
            7 => Sawtooth,
            _ => Sigmoid
        };
    }
    
    private static float Sigmoid(float x) => 2f / (1f + MathF.Exp(-2 * x)) - 1;
    private static float Sin(float value) => MathF.Sin(value); 
    private static float Cos(float value) => MathF.Cos(value);
    private static float NestedSin(float value) => MathF.Sin(value) + MathF.Sin(value * MathF.PI / 2);
    private static float NestedCos(float value) => MathF.Cos(value) + MathF.Cos(value * MathF.PI / 2);
    private static float CosSin2(float value) => MathF.Cos(value) + MathF.Sin(value * MathF.PI / 2);
    private static float SinCos2(float value) => MathF.Sin(value) + MathF.Cos(value * MathF.PI / 2);
    private static float Sawtooth(float x) => 2 * (x / (2f * MathF.PI) - MathF.Abs(x / (2f * MathF.PI) + 1f / 2f));
    
    
    private static readonly ISettingsBuilder<Spectrogram, Spectrogram> SETTINGS = SettingsBuilder<Spectrogram>.Build("spectrogram", 
        SettingsNode<Spectrogram>.New("spectrogram")
            .Group(IImageGenerator.UniversalSeedInput())
            .Argument("width", Arguments.Integer(32))
            .Argument("height", Arguments.Integer(32))
            .Argument("line_lifetime", Arguments.Integer(0))
            .Argument("step", Arguments.Double(0))
            .OnParse(cin => new Spectrogram(
                (int)cin["width"].GetParsedValue(), 
                (int)cin["height"].GetParsedValue(), 
                (int)cin["seed"].GetParsedValue(), 
                (int)cin["line_lifetime"].GetParsedValue(), 
                (int)(1 / (float)cin["step"].GetParsedValue()))
            )
    );

    public static SettingsBuilder<Spectrogram> GetSettings() {
        return (SettingsBuilder<Spectrogram>)SETTINGS.Clone();
    }

    public IImageGenerator GetNextGenerator(int i) {
        Spectrogram copy = (Spectrogram)MemberwiseClone();
        copy.seed = i;
        return copy;
    }
}