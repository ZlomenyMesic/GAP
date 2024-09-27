//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

namespace GAP;

class Program {

    public const string PROJECT_ID = "gap";
    
    static int Main() {
        //Console.WriteLine("Hello, World");

        DeepDream.RunGeneratorRandom(4);
        //DeepDream.RunGeneratorCustom("mixed3", "mixed2", "mixed5");

        return 0;
    }
}