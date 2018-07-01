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
	public class DoubleParseTests : CodeFixVerifier
	{
		[TestMethod]
		public void DoubleParseAnalyzer_ParseWithInvarientCulture_Ignore()
		{
			var test = @"
using System;
using System.Globalization;
namespace ConsoleApplication1
{
	class TypeName
	{   
		static void Main(string[] args)
		{
			double parsedDouble = double.Parse(""1.1"", NumberStyles.Any, CultureInfo.InvariantCulture);
		}
	}
}";


			VerifyCSharpDiagnostic(test);
		}

		[TestMethod]
		public void DoubleParseAnalyzer_ParseWithoutInvarientCulture_ProposeFix()
		{
			var test = @"
using System;
using System.Globalization;
namespace ConsoleApplication1
{
	class TypeName
	{   
		static void Main(string[] args)
		{
			double parsedDouble = double.Parse(""1.1"");
		}
	}
}";

			var expected = new DiagnosticResult
			{
				Id = "DoubleParseInvariantCulture",
				Message = String.Format("Can use InvariantCulture for parsing"),
				Severity = DiagnosticSeverity.Warning,
				Locations =
					new[] { new DiagnosticResultLocation("Test0.cs", 10, 26) }
			};

			VerifyCSharpDiagnostic(test, expected);
		}

		[TestMethod]
		public void DoubleParseAnalyzer_TryParseWithInvarientCulture_Ignore()
		{
			var test = @"
using System;
using System.Globalization;
namespace ConsoleApplication1
{
	class TypeName
	{   
		static void Main(string[] args)
		{
			bool didParse = double.TryParse(""1.1"", NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedDouble);
		}
	}
}";


			VerifyCSharpDiagnostic(test);
		}

		[TestMethod]
		public void DoubleParseAnalyzer_TryParseWithoutInvarientCulture_ProposeFix()
		{
			var test = @"
using System;
using System.Globalization;
namespace ConsoleApplication1
{
	class TypeName
	{   
		static void Main(string[] args)
		{
			bool didParse = double.TryParse(""1.1"", out double parsedDouble);
		}
	}
}";

			var expected = new DiagnosticResult
			{
				Id = "DoubleParseInvariantCulture",
				Message = String.Format("Can use InvariantCulture for parsing"),
				Severity = DiagnosticSeverity.Warning,
				Locations =
					new[] { new DiagnosticResultLocation("Test0.cs", 10, 20) }
			};

			VerifyCSharpDiagnostic(test, expected);
		}

		protected override CodeFixProvider GetCSharpCodeFixProvider()
		{
			return null;
		}

		protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
		{
			return new DoubleParseAnalyzer();
		}
	}
}
