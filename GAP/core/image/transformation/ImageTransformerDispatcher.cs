//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.exceptions;
using GAP.util.registries;

namespace GAP.core.image.transformation;

/// <summary>
/// Image Transformer Dispatcher <br/>
/// registers new <see cref="ImageTransformer"/> classes using the <see cref="ClassRegistry{T}"/> into the
/// <see cref="ImageTransformerRegistry"/> class registry
/// </summary>
public class ImageTransformerDispatcher {
    private string projectId { get; set; }
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
        this.projectId = projectId;
    }
    
    /// <summary>
    /// registers new <see cref="ImageTransformer"/>-implementing class
    /// </summary>
    /// <param name="id">id of the transformer class</param>
    /// <param name="type"><c>typeof(&lt;YourClassName&gt;)</c></param>
    public void Register(string id, Type type) {
        ImageTransformerRegistry.Register($"{projectId}:{id}", type);
    }
}