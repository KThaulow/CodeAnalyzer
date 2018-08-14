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
	public class DateTimeStringAnalyzerTests : CodeFixVerifier
	{
		[TestMethod]
		public void DateTimeStringAnalyzer_ToStringWitInvarientCulture_Ignore()
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
			var date = DateTime.UtcNow.ToString(""yyyy/MM/dd"", CultureInfo.InvariantCulture);
		}
	}
}";

			VerifyCSharpDiagnostic(test);
		}

		[TestMethod]
		public void DateTimeStringAnalyzer_ToStringWithoutInvarientCulture_ProposeFix()
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
			var date = DateTime.UtcNow.ToString(""yyyy/MM/dd"");
		}
	}
}";

			var expected = new DiagnosticResult
			{
				Id = "DateTimeInvariantCulture",
				Message = String.Format("Use InvariantCulture for printing date"),
				Severity = DiagnosticSeverity.Warning,
				Locations =
					new[] { new DiagnosticResultLocation("Test0.cs", 10, 15) }
			};

			VerifyCSharpDiagnostic(test, expected);
		}



		protected override CodeFixProvider GetCSharpCodeFixProvider()
		{
			return null;
		}

		protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
		{
			return new DateTimeStringAnalyzer();
		}
	}
}
