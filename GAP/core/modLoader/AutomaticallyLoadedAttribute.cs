//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

namespace GapCore.modLoader;

/// <summary>
/// Automatically Loaded Attribute <br/>
/// if set to true, the mod's contents will be loaded automatically
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AutomaticallyLoadedAttribute : Attribute {
    public readonly bool LoadAutomatically;
    
    public AutomaticallyLoadedAttribute(bool loadAutomatically = true) {
        LoadAutomatically = loadAutomatically;
    }
}