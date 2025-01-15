//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using NeoKolors.Common;
using NeoKolors.Settings;
using Color = System.Drawing.Color;

namespace GAP.core.image.generation.generators;

public class Spectrogram : IBatchableGenerator<Spectrogram> {
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Seed { get; private set; }
    
    public int LineLifetime { get; private set; }
    public int Step { get; private set; }
    

    /// <summary>
    /// 2d spectrogram line image generator
    /// </summary>
    /// <param name="width">image width</param>
    /// <param name="height">image height</param>
    /// <param name="seed">seed</param>
    /// <param name="lineLifetime">how long will the line be</param>
    /// <param name="step">idk what that is</param>
    public Spectrogram(int width, int height, int seed, int lineLifetime = 15000, int step = 200) {
        Width = width;
        Height = height;
        Seed = seed;
        LineLifetime = lineLifetime;
        Step = step;
    }
    
    public Spectrogram() {}

    public Bitmap GenerateImage() {
        Bitmap bmp = new Bitmap(Width, Height);
        Random rnd = new Random(Seed);

        float xFrequency = rnd.NextSingle();
        float yFrequency = rnd.NextSingle();
        int xOffset = Width / 2;
        int yOffset = Height / 2; 
        float factor = Width > Height ? (float)Width * 1/3 : (float)Height * 1/3;
        float startOffset = rnd.NextSingle();
        Func<float, float> xFunc = GetFunc();
        Func<float, float> yFunc = GetFunc();
        
        var a = (rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle());
        var b = (rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle());
        var c = (rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle());
        var d = (rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle());
        
        Color background = Color.FromArgb(255, 16, 16 ,16);
        Graphics g = Graphics.FromImage(bmp);
        g.Clear(background);
        
        for (float j = startOffset; j < LineLifetime / (float)Step + startOffset; j += 1 / (float)Step) {
            bmp.SetPixel((int)(xOffset + xFunc(xFrequency * j) * factor), 
                (int)(yOffset + yFunc(yFrequency * j) * factor), ColorPalette.GenerateColorAtX(a, b, c, d, j));
        }
        
        return bmp;
    }

    ISettingsBuilder<IImageGenerator> IImageGenerator.GetSettings() => GetSettings();


    private Func<float, float> GetFunc() {
        Random rnd = new Random(Seed);
        
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
    
    
    private static readonly ISettingsBuilder<Spectrogram> SETTINGS = SettingsBuilder<Spectrogram>.Build("spectrogram", 
        SettingsNode<Spectrogram>.New("spectrogram")
            .Group(IImageGenerator.UniversalSeedInput())
            .Argument("width", Arguments.Integer(32))
            .Argument("height", Arguments.Integer(32))
            .Argument("line_lifetime", Arguments.Integer(0))
            .Argument("step", Arguments.Double(0))
            .Constructs(cin => new Spectrogram(
                (int)cin["width"].Get(), 
                (int)cin["height"].Get(), 
                (int)cin["seed"].Get(), 
                (int)cin["line_lifetime"].Get(), 
                (int)(1 / (float)cin["step"].Get()))
            )
    );

    public SettingsBuilder<Spectrogram> GetSettings() {
        return (SettingsBuilder<Spectrogram>)SETTINGS.Clone();
    }
    

    IImageGenerator IBatchableGenerator.GetNextGenerator(int i) => GetNextGenerator(i);

    public Spectrogram GetNextGenerator(int i) {
        Spectrogram copy = (Spectrogram)MemberwiseClone();
        copy.Seed = i;
        return copy;
    }
}