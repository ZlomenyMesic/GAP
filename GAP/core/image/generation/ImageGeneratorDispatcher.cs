//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.exceptions;
using GAP.util.registries;

namespace GAP.core.image.generation;

/// <summary>
/// Image Generator Dispatcher <br/>
/// registers new <see cref="ImageGenerator"/> classes using the <see cref="ClassRegistry{T}"/> into the
/// <see cref="ImageGeneratorRegistry"/> class registry
/// </summary>
public class ImageGeneratorDispatcher {
    private string projectId { get; set; }
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
        this.projectId = projectId;
    }
    
    /// <summary>
    /// registers new <see cref="ImageGenerator"/>-implementing class
    /// </summary>
    /// <param name="id">id of the generator class</param>
    /// <param name="type"><c>typeof(&lt;YourClassName&gt;)</c></param>
    public void Register(string id, Type type) {
        ImageGeneratorRegistry.Register($"{projectId}:{id}", type);
    }
}