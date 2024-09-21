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

        //DeepDream.RunGenerator();
        DeepDream.RunGeneratorRandom(4);

        return 0;
    }
}