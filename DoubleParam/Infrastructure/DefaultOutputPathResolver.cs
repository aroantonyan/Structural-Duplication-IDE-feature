using DoubleParam.Abstractions;

namespace DoubleParam.Infrastructure;

public sealed class DefaultOutputPathResolver : IOutputPathResolver
{
    public (string? inputPath, bool inPlace, string? outputPath, string? error) Resolve(string[] args)
    {
        if (args.Length is < 1 or > 3)
        {
            return (null, false, null,
                "Usage:\n  dotnet run -- <input.cs> [--inplace | -o <output.cs>]");
        }

        var inputPath = args[0];
        var inPlace = Array.IndexOf(args, "--inplace") >= 0;

        if (!File.Exists(inputPath))
            return (null, false, null, $"File not found: {inputPath}");

        string? outputPath = null;

        if (inPlace) return (inputPath, inPlace, outputPath, null);
        var outIndex = Array.IndexOf(args, "-o");
        if (outIndex >= 0 && outIndex + 1 < args.Length)
        {
            outputPath = args[outIndex + 1];
        }
        else
        {
            var dir = Path.GetDirectoryName(inputPath)!;
            var nameNoExt = Path.GetFileNameWithoutExtension(inputPath);

            var baseName = nameNoExt.EndsWith("Out", StringComparison.Ordinal)
                ? nameNoExt
                : nameNoExt + "Out";

            outputPath = Path.Combine(dir, baseName + ".cs");
        }

        return (inputPath, inPlace, outputPath, null);
    }
}