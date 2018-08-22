using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using CodeAnalyzer.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TimeSpanFormatAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "AN0009";
        private const string Title = "Abnormal timspan formatting";
        private const string MessageFormat = "Check timespan formatting";
        private const string Description = "Check timespan formatting";
        private const string Category = "Usage";

        private const string MessageYear = "Year not valid in timespan formatting";
        private const string MessageMonthSuggestion = "Month not valid in timespan formatting. Did you mean 'm' (minutes)?";
        private const string MessageMonth = "Month not valid in timespan formatting";

        private static DiagnosticDescriptor s_DefaultRule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
        private static DiagnosticDescriptor s_YearRule = new DiagnosticDescriptor(DiagnosticId, Title, MessageYear, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
        private static DiagnosticDescriptor s_MonthSuggestionRule = new DiagnosticDescriptor(DiagnosticId, Title, MessageMonthSuggestion, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
        private static DiagnosticDescriptor s_MonthRule = new DiagnosticDescriptor(DiagnosticId, Title, MessageMonth, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);


        private static readonly List<TimeSpanPattern> s_ForbiddenTimeSpanPatterns = new List<TimeSpanPattern>()
        {
            new TimeSpanPattern(new Regex("y"), s_YearRule),
            new TimeSpanPattern(new Regex("Y"), s_YearRule),
            new TimeSpanPattern(new Regex("yy"), s_YearRule),
            new TimeSpanPattern(new Regex("yyy"), s_YearRule),
            new TimeSpanPattern(new Regex("yyyy"), s_YearRule),
            new TimeSpanPattern(new Regex("M"), s_MonthSuggestionRule),
            new TimeSpanPattern(new Regex("MM"), s_MonthSuggestionRule),
            new TimeSpanPattern(new Regex("MMM"), s_MonthRule),
            new TimeSpanPattern(new Regex("MMMM"), s_MonthRule)
        };

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(s_DefaultRule, s_YearRule, s_MonthSuggestionRule, s_MonthRule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(startContext =>
            {
                INamedTypeSymbol timeSpanSymbol = startContext.Compilation.GetTypeByMetadataName("System.TimeSpan");

                if (timeSpanSymbol is null)
                {
                    return;
                }

                startContext.RegisterSyntaxNodeAction(
                    nodeContext => AnalyzeInvocationExpressionSyntax(nodeContext, timeSpanSymbol),
                    SyntaxKind.InvocationExpression);
            });
        }

        private static void AnalyzeInvocationExpressionSyntax(SyntaxNodeAnalysisContext context, INamedTypeSymbol timeSpanSymbol)
        {
            var invocationExpressionSyntax = (InvocationExpressionSyntax)context.Node;

            ISymbol symbol = context.SemanticModel.GetSymbol(invocationExpressionSyntax, context.CancellationToken);

            if (symbol != null)
            {
                INamedTypeSymbol containingType = symbol.ContainingType;

                if (containingType?.Equals(timeSpanSymbol) == true)
                {
                    if (symbol.Kind == SymbolKind.Method
                        && (symbol.Name == "ToString"))
                    {
                        SeparatedSyntaxList<ArgumentSyntax> arguments = invocationExpressionSyntax.ArgumentList.Arguments;

                        if (!arguments.Any())
                            return;

                        foreach (var argument in arguments)
                        {
                            if (argument.Expression is LiteralExpressionSyntax literalExpressionSyntax)
                            {
                                foreach (var pattern in s_ForbiddenTimeSpanPatterns)
                                {
                                    if (pattern.Pattern.IsMatch(literalExpressionSyntax.Token.ValueText))
                                    {
                                        context.ReportDiagnostic(Diagnostic.Create(pattern.Rule, context.Node.GetLocation()));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
