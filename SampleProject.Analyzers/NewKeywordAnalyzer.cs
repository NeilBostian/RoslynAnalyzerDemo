using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace SampleProject.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NewKeywordAnalyzer : DiagnosticAnalyzer
    {
        private const string DiagnosticId = "Demo1001";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = "New keyword on member or method";
        private static readonly LocalizableString MessageFormat = "The `new` keyword should not be used on methods, properties, or fields.";
        private static readonly LocalizableString Description = null;

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, "Category", DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method, SymbolKind.Property);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var baseType = context.Symbol.ContainingType.BaseType;

            if (baseType == null || baseType.SpecialType == SpecialType.System_Object)
            {
                return;
            }

            if (context.Symbol.Is<MethodDeclarationSyntax>())
            {
                var method = context.Symbol.Convert<MethodDeclarationSyntax>();

                if (method.Modifiers.Any(d => d.IsKind(SyntaxKind.NewKeyword)))
                {
                    var location = method.Modifiers.First(d => d.IsKind(SyntaxKind.NewKeyword)).GetLocation();
                    var diagnostic = Diagnostic.Create(Rule, location, context.Symbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }

            if (context.Symbol.Is<PropertyDeclarationSyntax>())
            {
                var property = context.Symbol.Convert<PropertyDeclarationSyntax>();

                if (property.Modifiers.Any(d => d.IsKind(SyntaxKind.NewKeyword)))
                {
                    var location = property.Modifiers.First(d => d.IsKind(SyntaxKind.NewKeyword)).GetLocation();
                    var diagnostic = Diagnostic.Create(Rule, location, context.Symbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
