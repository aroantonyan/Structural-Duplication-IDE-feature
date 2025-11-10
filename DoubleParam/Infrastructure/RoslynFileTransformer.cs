using DoubleParam.Abstractions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Host.Mef;

namespace DoubleParam.Infrastructure;

public sealed class RoslynFileTransformer(IEnumerable<ICompilationUnitTransformer> transformers) : IFileTransformer
{
    public async Task<string> TransformAsync(string inputPath, CancellationToken ct = default)
    {
        if (!string.Equals(Path.GetExtension(inputPath), ".cs", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException($"Input must be a .cs file, got: {inputPath}");

        var source = await File.ReadAllTextAsync(inputPath, ct);

        var tree = CSharpSyntaxTree.ParseText(source, cancellationToken: ct);
        var root = (CompilationUnitSyntax)await tree.GetRootAsync(ct);
        
        var current = transformers.Aggregate(root, (current1, t) => t.Transform(current1));
        
        var workspace = new AdhocWorkspace(MefHostServices.DefaultHost);
        var formatted = (CompilationUnitSyntax)
            Microsoft.CodeAnalysis.Formatting.Formatter.Format(current, workspace, cancellationToken: ct);

        return formatted.ToFullString();
    }
}