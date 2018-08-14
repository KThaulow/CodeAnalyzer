using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace CodeAnalyzer.Analyzers
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class DateTimeStringAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "DateTimeInvariantCulture";
		private const string Title = "InvariantCulture for DateTime";
		private const string MessageFormat = "Use InvariantCulture for printing date";
		private const string Description = "Use InvariantCulture when calling ToString() on DateTime";
		private const string Category = "Usage";

		private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

		public override void Initialize(AnalysisContext context)
		{
			context.RegisterCompilationStartAction(startContext =>
			{
				INamedTypeSymbol dateTimeSymbol = startContext.Compilation.GetTypeByMetadataName("System.DateTime");

				if (dateTimeSymbol != null)
				{
					startContext.RegisterSyntaxNodeAction(
						nodeContext => AnalyzeInvocationExpressionSyntax(nodeContext, dateTimeSymbol),
						SyntaxKind.InvocationExpression);
				}
			});
		}

		private static void AnalyzeInvocationExpressionSyntax(SyntaxNodeAnalysisContext context, INamedTypeSymbol doubleSymbol)
		{
			var invocationExpressionSyntax = (InvocationExpressionSyntax)context.Node;

			ISymbol symbol = context.SemanticModel.GetSymbol(invocationExpressionSyntax, context.CancellationToken);

			INamedTypeSymbol containingType = symbol.ContainingType;

			if (containingType?.Equals(doubleSymbol) == true)
			{
				if (symbol.Kind == SymbolKind.Method
					&& (symbol.Name == "ToString"))
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
