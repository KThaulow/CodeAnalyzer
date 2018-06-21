﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading;

namespace CodeAnalyzer
{
	public static class CSharpExtensions
	{
		public static ITypeSymbol GetTypeSymbol(
		   this SemanticModel semanticModel,
		   ExpressionSyntax expression,
		   CancellationToken cancellationToken = default(CancellationToken))
		{
			return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
				.GetTypeInfo(semanticModel, expression, cancellationToken)
				.Type;
		}

		public static ISymbol GetSymbol(
		  this SemanticModel semanticModel,
		  ExpressionSyntax expression,
		  CancellationToken cancellationToken = default(CancellationToken))
		{
			return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
				.GetSymbolInfo(semanticModel, expression, cancellationToken)
				.Symbol;
		}

		public static ISymbol GetSymbol(
			this SemanticModel semanticModel,
			AttributeSyntax attribute,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			return Microsoft.CodeAnalysis.CSharp.CSharpExtensions
				.GetSymbolInfo(semanticModel, attribute, cancellationToken)
				.Symbol;
		}
	}
}
