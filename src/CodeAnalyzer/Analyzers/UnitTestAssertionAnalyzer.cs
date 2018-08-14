﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CodeAnalyzer.Analyzers
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class UnitTestAssertionAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "UnitTestAssertion";
		private const string Title = "Unit test without assertion";
		private const string MessageFormat = "Unit test should contain an assertion";
		private const string Description = "Unit test should contain an assertion";
		private const string Category = "Usage";
		private const int MAX_RECURSIVE_CALLS = 5;

		private static List<string> s_AssertTokens = new List<string>()
		{
			"Should",
			"Assert"
		};

		private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: Description);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

		public override void Initialize(AnalysisContext context)
		{
			context.RegisterSyntaxNodeAction(AnalyzeTestMethodName, SyntaxKind.MethodDeclaration);
		}

		private static void AnalyzeTestMethodName(SyntaxNodeAnalysisContext context)
		{
			var methodDeclarationSyntax = (MethodDeclarationSyntax)context.Node;

			// Check if method contains assertion
			if (methodDeclarationSyntax.AttributeLists.Any(e => (e is AttributeListSyntax attributeList)
				&& attributeList.Attributes.Any(u => u.Name.TryGetInferredMemberName() == "TestMethod")))
			{
				int resursiveCallCounter = 0;

				if (IsAssertionMethod(methodDeclarationSyntax, context, resursiveCallCounter))
				{
					return;
				}

				context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation()));
			}
		}

		private static bool IsAssertionMethod(MethodDeclarationSyntax methodDeclarationSyntax, SyntaxNodeAnalysisContext context, int resursiveCallCounter)
		{
			if (resursiveCallCounter > MAX_RECURSIVE_CALLS)
			{
				return true; // If the search have treversed more than 5 levels in a method, ignore this method
			}

			resursiveCallCounter++;
			foreach (var statement in methodDeclarationSyntax.Body.Statements)
			{
				if (IsAssertionStatement(statement, context, resursiveCallCounter))
				{
					return true;
				}
			}

			return false;
		}

		private static bool IsAssertionStatement(StatementSyntax statement, SyntaxNodeAnalysisContext context, int resursiveCallCounter)
		{
			if (statement is ExpressionStatementSyntax expressionStatementSyntax)
			{
				// Check is the expression if an assertion
				var expressionDescendants = expressionStatementSyntax.Expression.DescendantNodes();
				if (expressionDescendants.Any(e => e is IdentifierNameSyntax identifierNameSyntax
				&& s_AssertTokens.Any(i => i == identifierNameSyntax.Identifier.ValueText)))
				{
					return true;
				}

				// Check if any invoked methods are containing assertions
				var methodSymbol = context.SemanticModel.GetSymbolInfo(expressionStatementSyntax.Expression, context.CancellationToken).Symbol as IMethodSymbol;

				if (methodSymbol != null)
				{
					var syntaxReference = methodSymbol.DeclaringSyntaxReferences.FirstOrDefault();
					if (syntaxReference != null)
					{
						var declaration = syntaxReference.GetSyntax(context.CancellationToken);
						if (declaration is MethodDeclarationSyntax methodDeclarationSyntax
							&& methodDeclarationSyntax.Modifiers.Any(e => e.Kind() == SyntaxKind.PrivateKeyword)
							&& IsAssertionMethod(methodDeclarationSyntax, context, resursiveCallCounter))
						{
							return true;
						}
					}
				}
			}

			return false;
		}
	}
}