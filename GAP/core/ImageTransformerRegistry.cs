//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

using GAP.util.registries;
using GapCore.util.registries.exceptions;

namespace GapCore;

/// <summary>
/// Image Transformer Registry <br/>
/// holds references to all registered transformers using the <see cref="TypeRegistry{T}"/> class registry class
/// </summary>
internal abstract class ImageTransformerRegistry : TypeRegistry<IImageTransformer> {
   
    /// <summary>
    /// registers new <see cref="IImageTransformer"/>-implementing class using the
    /// <see cref="TypeRegistry{T}.BaseRegister"/> method
    /// </summary>
    /// <param name="id">id of the class</param>
    /// <param name="type"><c>typeof(&lt;YourClassName&gt;)</c></param>
    internal static Type Register(string id, Type type) {
        var c = type.GetConstructor([]);
        if (c == null) {
            throw new RegistryInvalidTypeException(type);
        }
        
        return BaseRegister(id, type);
    }
    
    /// <summary>
    /// registers new <see cref="IImageTransformer"/>-implementing class using the
    /// <see cref="TypeRegistry{T}.BaseRegister"/> method
    /// </summary>
    /// <param name="id">id of the class</param>
    internal static Type Register<T>(string id) {
        var c = typeof(T).GetConstructor([]);
        if (c == null) {
            throw new RegistryInvalidTypeException(typeof(T));
        }
        
        return BaseRegister(id, typeof(T));
    }
    
    /// <summary>
    /// returns new instance of the searched <see cref="IImageTransformer"/>-implementing class
    /// </summary>
    /// <param name="id">id of the searched class</param>
    /// <exception cref="NullReferenceException">
    /// registered reference to class is null</exception>
    /// <exception cref="KeyNotFoundException">no class with id of <see cref="id"/> was not found</exception>
    public static Type Get(string id) {
        if (!REGISTRY.TryGetValue(id, out Type? value))
            throw new KeyNotFoundException($"Could not find registry object \'{id}\'.");

        return value;
    }
}