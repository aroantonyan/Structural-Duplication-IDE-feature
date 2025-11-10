using DoubleParam.Abstractions;
using DoubleParam.Naming;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoubleParam.Transformers;

public sealed class SingleParamMethodDuplicator(IParameterNameSuggester suggester)
    : CSharpSyntaxRewriter, ICompilationUnitTransformer
{
    public CompilationUnitSyntax Transform(CompilationUnitSyntax root)
        => (CompilationUnitSyntax)Visit(root);

    public override SyntaxNode? VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        var name = node.Identifier.Text;

        if (!string.IsNullOrWhiteSpace(name) && !name.EndsWith("Out", StringComparison.Ordinal))
        {
            node = node.WithIdentifier(SyntaxFactory.Identifier(name + "Out"));
        }

        return base.VisitClassDeclaration(node);
    }

    public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        var parameters = node.ParameterList.Parameters;

        if (parameters.Count != 1) return base.VisitMethodDeclaration(node);
        var p0 = parameters[0];
        var type = p0.Type;

        var newName = suggester.Suggest(p0.Identifier.Text, type);

        var p1 = p0
            .WithIdentifier(SyntaxFactory.Identifier(newName))
            .WithDefault(null);

        var newParams = parameters.Add(p1);
        var newParamList = node.ParameterList.WithParameters(newParams);

        node = node.WithParameterList(newParamList);
        return node;
    }
}