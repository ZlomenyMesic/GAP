//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

namespace GapCore.util.registries.exceptions;

public class RegistryInvalidTypeException : Exception {
    public RegistryInvalidTypeException(Type type, Type baseType) : 
        base($"Registered type {type.FullName} is not a {baseType.FullName}") { }
    
    public RegistryInvalidTypeException(Type type) : 
        base($"Registered type {type.FullName} does not have an empty constructor.") { }
}