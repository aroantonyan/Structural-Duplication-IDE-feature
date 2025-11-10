namespace DoubleParam.Abstractions;

public interface IFileTransformer
{
    Task<string> TransformAsync(string inputPath, CancellationToken ct = default);
    
}