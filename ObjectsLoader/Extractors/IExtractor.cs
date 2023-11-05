namespace ObjectsLoader.Extractors;

public interface IExtractor<T>
{
    public Task<List<T>> Extract();
}