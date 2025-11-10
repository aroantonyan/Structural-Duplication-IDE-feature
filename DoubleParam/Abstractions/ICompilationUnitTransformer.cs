using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoubleParam.Abstractions;

public interface ICompilationUnitTransformer
{
    CompilationUnitSyntax Transform(CompilationUnitSyntax root);
}