namespace DoubleParam.Abstractions;


public interface IOutputPathResolver
{
    (string? inputPath, bool inPlace, string? outputPath, string? error) Resolve(string[] args);
}