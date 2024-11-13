namespace GAP.core.image.generation;

public interface IBatchableGenerator : IImageGenerator {
    public IImageGenerator GetNextGenerator(int i);
}