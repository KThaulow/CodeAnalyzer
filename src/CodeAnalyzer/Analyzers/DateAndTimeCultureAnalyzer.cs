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
	public class DateAndTimeCultureAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "AN0007";
		private const string Title = "InvariantCulture for date/time";
		private const string MessageFormat = "Use InvariantCulture for printing date/time";
		private const string Description = "Use InvariantCulture when calling ToString() on DateTime";
		private const string Category = "Usage";

		private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

		public override void Initialize(AnalysisContext context)
		{
			context.RegisterCompilationStartAction(startContext =>
			{
				INamedTypeSymbol dateTimeSymbol = startContext.Compilation.GetTypeByMetadataName("System.DateTime");
				INamedTypeSymbol dateTimeOffsetSymbol = startContext.Compilation.GetTypeByMetadataName("System.DateTimeOffset");
				INamedTypeSymbol timeSpanSynbol = startContext.Compilation.GetTypeByMetadataName("System.TimeSpan");

				var registeredSymbols = new List<INamedTypeSymbol>()
				{
					dateTimeSymbol,
					dateTimeOffsetSymbol,
					timeSpanSynbol
				};

				if (dateTimeSymbol != null)
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

			if (symbol != null)
			{
				INamedTypeSymbol containingType = symbol.ContainingType;

				foreach (var registeredSymbol in registeredSymbols)
				{
					if (containingType?.Equals(registeredSymbol) == true)
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
	}
}
