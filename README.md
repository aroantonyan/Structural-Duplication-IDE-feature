What

CLI tool using the Microsoft Roslyn SDK to duplicate the single parameter of C# methods and suggest a new name.

Abstractions/ – IFileTransformer, ICompilationUnitTransformer, IOutputPathResolver, IParameterNameSuggester

Infrastructure/ – RoslynFileTransformer, DefaultOutputPathResolver

Transformers/ – SingleParamMethodDuplicator

Naming/ – DefaultParameterNameSuggester

Usage
dotnet run -- <INPUT.cs> [--inplace | --out <OUTPUT.cs>]

Test
dotnet run -- ../DoubleParamTest/Test.cs
