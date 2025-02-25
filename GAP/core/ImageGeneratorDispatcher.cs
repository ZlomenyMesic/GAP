//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

using GAP.util.registries;
using GapCore.util.exceptions;

namespace GapCore;

/// <summary>
/// Image Generator Dispatcher <br/>
/// registers new <see cref="IImageGenerator"/> classes using the <see cref="TypeRegistry{T}"/> into the
/// <see cref="ImageGeneratorRegistry"/> class registry
/// </summary>
public class ImageGeneratorDispatcher {
    private string ProjectId { get; set; }
    private static readonly List<string> REGISTERED_IDS = new();
    
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="projectId">id of project</param>
    /// <exception cref="DuplicateIdException">if the same project id has already been registered</exception>
    public ImageGeneratorDispatcher(string projectId) {
        
        foreach (var id in REGISTERED_IDS.Where(id => projectId == id)) {
            throw new DuplicateIdException(id);
        }
        
        REGISTERED_IDS.Add(projectId);
        ProjectId = projectId;
    }
    
    /// <summary>
    /// registers new <see cref="IImageGenerator"/>-implementing class
    /// </summary>
    /// <param name="id">id of the generator class</param>
    /// <param name="type"><c>typeof(&lt;YourClassName&gt;)</c></param>
    public void Register(string id, Type type) {
        ImageGeneratorRegistry.Register($"{ProjectId}:{id}", type);
    }
}