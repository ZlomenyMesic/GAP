//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

namespace GapCore.util.registries.exceptions;

public class RegistryItemNotFoundException : Exception {
    public RegistryItemNotFoundException(string id) : base($"Could not find registry object \'{id}\'.") { }
}