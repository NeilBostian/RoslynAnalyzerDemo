using Microsoft.CodeAnalysis;
using System.Linq;

namespace SampleProject.Analyzers
{
    internal static class AnalyzerExtensions
    {
        public static bool Is<T>(this ISymbol symbol) where T : class
            => symbol.DeclaringSyntaxReferences.Single().GetSyntax() is T;

        public static T Convert<T>(this ISymbol symbol) where T : class
            => symbol.DeclaringSyntaxReferences.Single().GetSyntax() as T;
    }
}
