using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoubleParam.Naming;

public sealed class DefaultParameterNameSuggester : IParameterNameSuggester
{
    public string Suggest(string oldName, TypeSyntax? type)
    {
        if (!string.IsNullOrWhiteSpace(oldName))
        {
            if (TryBumpNumericSuffix(oldName, out var bumped))
                return bumped;

            if (type is IdentifierNameSyntax id && !string.IsNullOrWhiteSpace(id.Identifier.Text))
                return "other" + ToPascal(id.Identifier.Text);

            return oldName + "2";
        }

        if (type is IdentifierNameSyntax id2 && !string.IsNullOrWhiteSpace(id2.Identifier.Text))
        {
            var t = id2.Identifier.Text;
            if (string.Equals(t, "string", StringComparison.OrdinalIgnoreCase)) return "text2";
            if (string.Equals(t, "int", StringComparison.OrdinalIgnoreCase)) return "value2";
            return "other" + ToPascal(t);
        }

        return "arg2";
    }

    private static bool TryBumpNumericSuffix(string name, out string? bumped)
    {
        bumped = null;
        var i = name.Length - 1;
        while (i >= 0 && char.IsDigit(name[i])) i--;
        if (i == name.Length - 1)
        {
            bumped = name + "2";
            return true;
        }
        var head = name[..(i + 1)];
        var digits = name[(i + 1)..];
        if (!int.TryParse(digits, out var n)) return false;
        bumped = head + (n + 1);
        return true;
    }

    private static string ToPascal(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        if (s.Length == 1) return char.ToUpperInvariant(s[0]).ToString();
        return char.ToUpperInvariant(s[0]) + s[1..];
    }
}