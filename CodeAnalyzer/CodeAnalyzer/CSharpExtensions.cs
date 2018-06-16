using Microsoft.CodeAnalysis;
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
	}
}
