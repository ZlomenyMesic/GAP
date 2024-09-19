//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

using GAP.core.image.generation.generators;
using System.Drawing;

namespace GAP;

class Program {
    static int Main() {
        //Console.WriteLine("Hello, World");

        //WhiteNoise noise = new(1000, 700, 69);
        //var img = noise.GenerateImage();
        //img.Save("whitenoise.jpg");

        DeepDream.RunGenerator();

        return 0;
    }
}