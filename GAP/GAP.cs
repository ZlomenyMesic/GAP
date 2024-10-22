//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//
//      founded 11.9.2024
//

using GAP;
using GAP.core.image.transformation;
using GAP.core.modLoader;

namespace GAP;

class GAP : Mod {

    public const string PROJECT_ID = "gap";
    
    static int Main() {

        // Mod loading
        // Totally harmless, and totally doesn't load itself as a mod
        //int modCount = ModLoader.LoadMods(".");

        DeepDream.RunGeneratorRandom(4);
        // DeepDream.RunGeneratorCustom("mixed3", "mixed2", "mixed5");

        return 0;
    }

    public string Register() {
        return PROJECT_ID;
    }

    public void Initialize() {
        DefaultRegistries.Register();
    }
}