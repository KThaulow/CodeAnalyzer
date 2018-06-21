using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace CodeAnalyzer.Analyzers
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class CircuitBreakerAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "CircuitBreaker";
		private const string Title = "Circuit breaker in loop";
		private const string MessageFormat = "Add circuit breaker to loop";
		private const string Description = "Add circuit breaker";
		private const string Category = "Usage";

		private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

		public override void Initialize(AnalysisContext context)
		{
			context.RegisterSyntaxNodeAction(AnalyzeWhileStatement, SyntaxKind.WhileStatement);
		}

		private static void AnalyzeWhileStatement(SyntaxNodeAnalysisContext context)
		{
			var whileStatement = (WhileStatementSyntax)context.Node;

			if (whileStatement.Statement is BlockSyntax blockSyntax)
			{
				foreach (var statement in blockSyntax.Statements)
				{
					if (statement is BreakStatementSyntax
						|| statement is ReturnStatementSyntax)
					{
						return;
					}
				}

				context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation()));
			}
		}
	}
}
