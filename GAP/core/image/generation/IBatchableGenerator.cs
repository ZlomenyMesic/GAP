namespace GAP.core.image.generation;

public interface IBatchableGenerator<TSelf> : IImageGenerator<TSelf>, IBatchableGenerator where TSelf : class, IImageGenerator<TSelf>  {
    public new TSelf GetNextGenerator(int i);
}

public interface IBatchableGenerator : IImageGenerator {
    public IImageGenerator GetNextGenerator(int i);
}