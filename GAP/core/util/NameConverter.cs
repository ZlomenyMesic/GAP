//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

namespace GAP.util;

public static class NameConverter {
    
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
    
    public static string CodeNameToId(string name) {
        string id = "";
        bool isFirst = true;
        
        foreach (var c in name) {
            if (char.IsUpper(c)) {
                id += isFirst ? "" : "_";
                id += char.ToLower(c);
            }
            else if (char.IsWhiteSpace(c)) {
                id += '_';
            }
            else if (char.IsDigit(c)) {
                id += c;
            }
            else {
                id += c;
            }
            
            isFirst = false;
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

    public static string EnumValueToName(string enumValue) {
        enumValue = enumValue.Replace("_", " ");
        enumValue = enumValue.ToLower();

        string name = "";
        
        name += Char.ToUpper(enumValue[0]);
        
        for (int i = 1; i < enumValue.Length; i++) {
            if (enumValue[i - 1] == ' ') name += Char.ToUpper(enumValue[i]);
            else name += enumValue[i];
        }
        
        return name;
    }
}