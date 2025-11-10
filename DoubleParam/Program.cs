using DoubleParam.Abstractions;
using DoubleParam.Infrastructure;
using DoubleParam.Naming;
using DoubleParam.Transformers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DoubleParam;

public abstract class Program
{
    public static async Task<int> Main(string[] args)
    {
        using var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<IParameterNameSuggester, DefaultParameterNameSuggester>();
                services.AddSingleton<ICompilationUnitTransformer, SingleParamMethodDuplicator>();
                services.AddSingleton<IFileTransformer, RoslynFileTransformer>();
                services.AddSingleton<IOutputPathResolver, DefaultOutputPathResolver>();
            })
            .Build();

        var pathResolver = host.Services.GetRequiredService<IOutputPathResolver>();
        var (inputPath, inPlace, outputPath, error) = pathResolver.Resolve(args);

        if (error is not null)
        {
            await Console.Error.WriteLineAsync(error);
            return error.StartsWith("Usage:") ? 1 : 2;
        }

        var pipeline = host.Services.GetRequiredService<IFileTransformer>();
        var transformed = await pipeline.TransformAsync(inputPath!);

        if (inPlace)
        {
            await File.WriteAllTextAsync(inputPath!, transformed);
            Console.WriteLine($"Saved in place: {inputPath}");
        }
        else
        {
            await File.WriteAllTextAsync(outputPath!, transformed);
            Console.WriteLine($"Saved: {outputPath}");
        }

        return 0;
    }
}