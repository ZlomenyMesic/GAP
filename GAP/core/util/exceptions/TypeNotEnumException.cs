//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

namespace GapCore.util.exceptions;

public class TypeNotEnumException : Exception {
    public TypeNotEnumException(Type type) : base($"Type '{type.Name}' is not a valid enum type.") { }    
}