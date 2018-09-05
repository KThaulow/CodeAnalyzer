using System.Collections.Immutable;
using CodeAnalyzer.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ListContainsAnalyzer : BaseDiagnosticAnalyzer
    {
        public const string DiagnosticId = "AN0010";
        private const string Title = "Use Contains instead of Linq";
        private const string MessageFormat = "Use Contains instead of Linq";
        private const string Description = "Use Contains instead of Linq";
        private const string Category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterSyntaxNodeAction(AnalyzePossibleContains, SyntaxKind.InvocationExpression);
        }

        private static void AnalyzePossibleContains(SyntaxNodeAnalysisContext context)
        {
            var invocationExpression = (InvocationExpressionSyntax)context.Node;

            if (invocationExpression is null)
                return;

            SemanticModel semanticModel = context.SemanticModel;
            IMethodSymbol methodSymbol = semanticModel.GetExtensionMethodSymbol(invocationExpression);

            if (methodSymbol is null)
                return;

            if (!SymbolUtility.IsLinqIEnumerableWithPredicate(methodSymbol, "Any"))
                return;

            context.ReportDiagnostic(Diagnostic.Create(Rule, invocationExpression.GetLocation()));
        }
    }
}
