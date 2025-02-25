//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

namespace GapCore.modLoader;

/// <summary>
/// Mod Interface <br/>
/// an interface all mod libraries must implement
/// </summary>
public interface IMod {

    /// <summary>
    /// method called at mod gathering
    /// </summary>
    /// <returns>mod id</returns>
    public string Register();
    
    /// <summary>
    /// method called at mod initialization
    /// </summary>
    public void Initialize();

    /// <summary>
    /// returns the description of the mod
    /// </summary>
    public string GetInfo();
}