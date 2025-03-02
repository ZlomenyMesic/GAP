//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

using GAP.util.registries;
using GapCore.util.exceptions;

namespace GapCore;

/// <summary>
/// Image Transformer Dispatcher <br/>
/// registers new <see cref="IImageTransformer"/> classes using the <see cref="TypeRegistry{T}"/> into the
/// <see cref="ImageTransformerRegistry"/> class registry
/// </summary>
public class ImageTransformerDispatcher {
    private string ProjectId { get; set; }
    private static readonly List<string> REGISTERED_IDS = new();
    
    
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="projectId">id of project</param>
    /// <exception cref="DuplicateIdException">if the same project id has already been registered</exception>
    public ImageTransformerDispatcher(string projectId) {
        foreach (var id in REGISTERED_IDS.Where(id => projectId == id)) {
            throw new DuplicateIdException(id);
        }
        
        REGISTERED_IDS.Add(projectId);
        ProjectId = projectId;
    }
    
    /// <summary>
    /// registers new <see cref="IImageTransformer"/>-implementing class
    /// </summary>
    /// <param name="id">id of the transformer class</param>
    /// <param name="type"><c>typeof(&lt;YourClassName&gt;)</c></param>
    public void Register(string id, Type type) {
        ImageTransformerRegistry.Register($"{ProjectId}:{id}", type);
    }
}