using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CodeAnalyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DoubleFloatParseAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "AN0003";
        private const string Title = "Use InvariantCulture when parsing a double/float";
        private const string MessageFormat = "Use InvariantCulture for double/float parsing";
        private const string Description = "Use InvariantCulture when parsing a double/float to string";
        private const string Category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(startContext =>
            {
                INamedTypeSymbol doubleSymbol = startContext.Compilation.GetTypeByMetadataName("System.Double");
                INamedTypeSymbol floatSymbol = startContext.Compilation.GetTypeByMetadataName("System.Single");
                var registeredSymbols = new INamedTypeSymbol[]
                {
                    doubleSymbol,
                    floatSymbol
                };

                if (registeredSymbols.Any())
                {
                    startContext.RegisterSyntaxNodeAction(
                        nodeContext => AnalyzeInvocationExpressionSyntax(nodeContext, registeredSymbols),
                        SyntaxKind.InvocationExpression);
                }
            });
        }

        private static void AnalyzeInvocationExpressionSyntax(SyntaxNodeAnalysisContext context, IEnumerable<INamedTypeSymbol> registeredSymbols)
        {
            var invocationExpressionSyntax = (InvocationExpressionSyntax)context.Node;

            ISymbol symbol = context.SemanticModel.GetSymbol(invocationExpressionSyntax, context.CancellationToken);

            if (symbol is null)
            {
                return;
            }

            INamedTypeSymbol containingType = symbol.ContainingType;

            foreach (var registeredSymbol in registeredSymbols)
            {
                if (containingType?.Equals(registeredSymbol) == true)
                {
                    if (symbol.Kind == SymbolKind.Method
                        && (symbol.Name == "Parse" || symbol.Name == "TryParse"))
                    {
                        SeparatedSyntaxList<ArgumentSyntax> arguments = invocationExpressionSyntax.ArgumentList.Arguments;

                        if (!arguments.Any())
                            return;

                        if (!arguments.Any(e => e.Expression.TryGetInferredMemberName() == "InvariantCulture"))
                        {
                            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation()));
                        }
                    }
                }
            }
        }
    }
}
