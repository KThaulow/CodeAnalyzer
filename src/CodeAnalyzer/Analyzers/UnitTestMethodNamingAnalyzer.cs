﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeAnalyzer.Analyzers
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class UnitTestMethodNamingAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "UnitTestMethodNaming";
		private const string Title = "Unit test method naming";
		private const string MessageFormat = "Use unit test method naming convention: [UnitToTest]_[Scenario]_[ExpectedOutcome]";
		private const string Description = "Use unit test method naming convention";
		private const string Category = "Style";

		private static Regex s_UnitTestNameRegex = new Regex("[A-Za-z0-9]+_[A-Za-z0-9]+_[A-Za-z0-9]+");

		private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: Description);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

		public override void Initialize(AnalysisContext context)
		{
			context.RegisterSyntaxNodeAction(AnalyzeTestMethodName, SyntaxKind.MethodDeclaration);
		}

		private static void AnalyzeTestMethodName(SyntaxNodeAnalysisContext context)
		{
			var methodDeclarationSyntax = (MethodDeclarationSyntax)context.Node;

			if (methodDeclarationSyntax.AttributeLists.Any(e => (e is AttributeListSyntax attributeList)
				&& attributeList.Attributes.Any(u => u.Name.TryGetInferredMemberName() == "TestMethod")))
			{
				var methodName = methodDeclarationSyntax.Identifier.ValueText;
				if (s_UnitTestNameRegex.IsMatch(methodName))
				{
					return;
				}

				context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation()));
			}
		}
	}
}