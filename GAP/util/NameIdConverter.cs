//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

namespace GAP.util;

public class NameIdConverter {
    
    public static string NameToId(string name) {
        string id = "";

        foreach (var c in name) {
            if (char.IsUpper(c)) {
                id += char.ToLower(c);
            }
            else if (char.IsWhiteSpace(c)) {
                id += '_';
            }
            else {
                id += c;
            }
        }
        
        return id;
    }

    public static string IdToName(string id, bool removeSourceName = true) {
        string name = "";

        int i = 0;

        while (id[i] != ':') {
            if (i == id.Length - 1) {
                i = 0;
                break;
            }
            
            i++;
        }

        name += char.ToUpper(id[++i]);
        
        for (i++; i < id.Length; i++) {
            if (id[i] == '_') {
                if (i == id.Length - 1) break;
                
                name += ' ';
                if (i + 1 < id.Length) {
                    name += char.ToUpper(id[++i]);
                }
            }
            else {
                name += id[i];
            }
        }
        
        return name;
    }
}