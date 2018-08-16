using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeAnalyzer.Analyzers
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class DateFormatAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "AN0008";
		private const string Title = "Abnormal date formatting";
		private const string MessageFormat = "Minutes (mm) formatted in date context instead of months (MM)";
		private const string Description = "Minutes (mm) formatted in date context instead of months (MM)";
		private const string Category = "Usage";

		private static readonly List<Regex> s_AbnormalDatePatterns = new List<Regex>()
		{
			new Regex("yyyy.mm.dd"),
			new Regex("mm.dd.yyyy"),
			new Regex("dd.mm.yyyy"),
			new Regex("yy.mm.dd"),
			new Regex("mm.dd.yy"),
			new Regex("dd.mm.yy")
		};

		private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

		public override void Initialize(AnalysisContext context)
		{
			context.RegisterCompilationStartAction(startContext =>
			{
				INamedTypeSymbol dateTimeSymbol = startContext.Compilation.GetTypeByMetadataName("System.DateTime");
				INamedTypeSymbol dateTimeOffsetSymbol = startContext.Compilation.GetTypeByMetadataName("System.DateTimeOffset");

				var registeredSymbols = new List<INamedTypeSymbol>()
				{
					dateTimeSymbol,
					dateTimeOffsetSymbol
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


							foreach (var argument in arguments)
							{
								if (argument.Expression is LiteralExpressionSyntax literalExpressionSyntax)
								{
									if (s_AbnormalDatePatterns.Any(e => e.IsMatch(literalExpressionSyntax.Token.ValueText)))
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

	}
}
