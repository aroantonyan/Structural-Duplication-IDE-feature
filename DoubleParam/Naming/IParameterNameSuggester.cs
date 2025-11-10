using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoubleParam.Naming;

public interface IParameterNameSuggester
{
    string Suggest(string oldName, TypeSyntax? type);
}