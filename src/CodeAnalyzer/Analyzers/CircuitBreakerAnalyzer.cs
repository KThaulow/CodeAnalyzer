using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalyzer.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CircuitBreakerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "AN0001";
        private const string Title = "Circuit breaker in loop";
        private const string MessageFormat = "Add circuit breaker to loop";
        private const string Description = "Add circuit breaker to loop";
        private const string Category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeWhileStatement, SyntaxKind.WhileStatement);
            context.RegisterSyntaxNodeAction(AnalyzeForStatement, SyntaxKind.ForStatement);
        }

        private static void AnalyzeWhileStatement(SyntaxNodeAnalysisContext context)
        {
            var whileStatement = (WhileStatementSyntax)context.Node;

            if (!(whileStatement.Statement is BlockSyntax))
            {
                return;
            }

            if (!HasCirCuitBreaker(whileStatement.Statement))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation()));
            }
        }

        private static void AnalyzeForStatement(SyntaxNodeAnalysisContext context)
        {
            var forStatement = (ForStatementSyntax)context.Node;

            if (forStatement.Incrementors.Count == 0
                && forStatement.Initializers.Count == 0
                && !forStatement.OpenParenToken.ContainsDirectives
                && !forStatement.FirstSemicolonToken.ContainsDirectives
                && !forStatement.SecondSemicolonToken.ContainsDirectives
                && !forStatement.CloseParenToken.ContainsDirectives)
            {
                if (!(forStatement.Statement is BlockSyntax))
                {
                    return;
                }

                if (!HasCirCuitBreaker(forStatement.Statement))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation()));
                }
            }
        }

        private static bool HasCirCuitBreaker(StatementSyntax statementSyntax)
        {
            if (!(statementSyntax is BlockSyntax blockSyntax))
            {
                return false;
            }

            foreach (var statement in blockSyntax.Statements)
            {
                if (statement is BreakStatementSyntax
                    || statement is ReturnStatementSyntax)
                {
                    return true;
                }
                else if (statement is IfStatementSyntax ifStatementSyntax)
                {
                    if (HasCirCuitBreaker(ifStatementSyntax.Statement)
                        || HasCirCuitBreaker(ifStatementSyntax?.Else?.Statement))
                    {
                        return true;
                    }
                }
                else if (HasCirCuitBreaker(statement))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
