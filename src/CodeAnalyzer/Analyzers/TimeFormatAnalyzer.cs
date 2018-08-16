using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace CodeAnalyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TimeFormatAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "AN0004";
        private const string Title = "24 hour format for times";
        private const string MessageFormat = "Use 24 hour time format";
        private const string Description = "Use 24 hour format instead of 12 hour format";
        private const string Category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(startContext =>
            {
                INamedTypeSymbol dateTimeSymbol = startContext.Compilation.GetTypeByMetadataName("System.DateTime");

                if (dateTimeSymbol is null)
                {
                    return;
                }

                startContext.RegisterSyntaxNodeAction(
                    nodeContext => AnalyzeInvocationExpressionSyntax(nodeContext, dateTimeSymbol),
                    SyntaxKind.InvocationExpression);
            });
        }

        private static void AnalyzeInvocationExpressionSyntax(SyntaxNodeAnalysisContext context, INamedTypeSymbol dateTimeSymbol)
        {
            var invocationExpressionSyntax = (InvocationExpressionSyntax)context.Node;

            ISymbol symbol = context.SemanticModel.GetSymbol(invocationExpressionSyntax, context.CancellationToken);

            if (symbol is null)
            {
                return;
            }

            INamedTypeSymbol containingType = symbol.ContainingType;

            if (containingType?.Equals(dateTimeSymbol) != true)
            {
                return;
            }

            if (symbol.Kind != SymbolKind.Method
                || (symbol.Name != "ToString"))
            {
                return;
            }

            SeparatedSyntaxList<ArgumentSyntax> arguments = invocationExpressionSyntax.ArgumentList.Arguments;

            if (!arguments.Any())
                return;

            foreach (var argument in arguments)
            {
                if (argument.Expression is LiteralExpressionSyntax literalExpressionSyntax)
                {
                    if (literalExpressionSyntax.Token.ValueText.Contains("hh"))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation()));
                    }
                }
            }
        }
    }
}
