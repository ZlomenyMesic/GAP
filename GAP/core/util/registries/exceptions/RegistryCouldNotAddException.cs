//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

namespace GapCore.util.registries.exceptions;

public class RegistryCouldNotAddException : Exception {
    public RegistryCouldNotAddException(string id, Type type) : 
        base($"Could not add registry object \'{id}\' of type \'{type}\' to a registry! " +
             $"Object with the same id already exists.") { }
}