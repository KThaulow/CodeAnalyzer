using CodeAnalyzer.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;

namespace CodeAnalyzer.Test
{
	[TestClass]
	public class CircuitBreakerUnitTest : CodeFixVerifier
	{
		[TestMethod]
		public void CircuitBreakerAnalyzer_WhileNoBreak_ProposeFix()
		{
			var test = @"
namespace ConsoleApplication1
{
	class TypeName
	{   
		static void Main(string[] args)
		{
			int i = 0;
			while(i < 10)
			{
				i--;
				i++;
			}
		}
	}
}";

			var expected = new DiagnosticResult
			{
				Id = "CircuitBreaker",
				Message = String.Format("Add circuit breaker to loop"),
				Severity = DiagnosticSeverity.Warning,
				Locations =
					new[] {
							new DiagnosticResultLocation("Test0.cs", 9,4)
						}
			};

			VerifyCSharpDiagnostic(test, expected);
		}

		[TestMethod]
		public void CircuitBreakerAnalyzer_WhileWithBreak_Ignore()
		{
			var test = @"
namespace ConsoleApplication1
{
	class TypeName
	{   
		static void Main(string[] args)
		{
			int i = 0;
			while(i < 10)
			{
				i--;
				i++;
				break;
			}
		}
	}
}";

			VerifyCSharpDiagnostic(test);
		}


		[TestMethod]
		public void CircuitBreakerAnalyzer_WhileWithReturn_Ignore()
		{
			var test = @"
namespace ConsoleApplication1
{
	class TypeName
	{   
		static void Main(string[] args)
		{
			int i = 0;
			while(i < 10)
			{
				i--;
				i++;
				return;
			}
		}
	}
}";

			VerifyCSharpDiagnostic(test);
		}


		[TestMethod]
		public void CircuitBreakerAnalyzer_ForWithoutIncrementerNoBreak_ProposeFix()
		{
			var test = @"
namespace ConsoleApplication1
{
	class TypeName
	{   
		static void Main(string[] args)
		{
			for(int i = i; i < 10;)
			{
				int o = 1;
			}
		}
	}
}";

			var expected = new DiagnosticResult
			{
				Id = "CircuitBreaker",
				Message = String.Format("Add circuit breaker to loop"),
				Severity = DiagnosticSeverity.Warning,
				Locations =
					new[] {
							new DiagnosticResultLocation("Test0.cs", 8,4)
						}
			};

			VerifyCSharpDiagnostic(test, expected);
		}

		[TestMethod]
		public void CircuitBreakerAnalyzer_ForWithIncrementerNoBreak_Ignore()
		{
			var test = @"
namespace ConsoleApplication1
{
	class TypeName
	{   
		static void Main(string[] args)
		{
			for(int i = i; i < 10; i++)
			{
				int o = 1;
			}
		}
	}
}";

			VerifyCSharpDiagnostic(test);
		}

		[TestMethod]
		public void CircuitBreakerAnalyzer_ForWithBreak_Ignore()
		{
			var test = @"
namespace ConsoleApplication1
{
	class TypeName
	{   
		static void Main(string[] args)
		{
			for(int i = i; i < 10;)
			{
				int o = 1;
				break;
			}
		}
	}
}";

			VerifyCSharpDiagnostic(test);
		}


		[TestMethod]
		public void CircuitBreakerAnalyzer_ForWithReturn_Ignore()
		{
			var test = @"
namespace ConsoleApplication1
{
	class TypeName
	{   
		static void Main(string[] args)
		{
			for(int i = i; i < 10;)
			{
				int o = 1;
				return;
			}
		}
	}
}";

			VerifyCSharpDiagnostic(test);
		}


		protected override CodeFixProvider GetCSharpCodeFixProvider()
		{
			return new ConstantFixProvider();
		}

		protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
		{
			return new CircuitBreakerAnalyzer();
		}

	}
}
