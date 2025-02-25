//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

namespace GapCore.modLoader;

[AttributeUsage(AttributeTargets.Class)]
public class ExcludeFromModLoadingAttribute : Attribute {
    public readonly bool Exclude;

    public ExcludeFromModLoadingAttribute(bool exclude = true) {
        Exclude = exclude;
    }
}