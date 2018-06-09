
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;

namespace CodeAnalyzer.Test
{
	[TestClass]
	public class DateTimeAnalyzerTests : CodeFixVerifier
	{
		[TestMethod]
		public void DateTimeAnalyzer_InitializeDateTimeNow_ProposeFix()
		{
			var test = @"
namespace ConsoleApplication1
{
	class TypeName
	{   
		static void Main(string[] args)
		{
			DateTime date = DateTime.Now;
		}
	}
}";

			var expected = new DiagnosticResult
			{
				Id = "DateTimeUTC",
				Message = String.Format("Can be made DateTime.UtcNow"),
				Severity = DiagnosticSeverity.Warning,
				Locations =
					new[] {
							new DiagnosticResultLocation("Test0.cs", 8,4)
						}
			};

			VerifyCSharpDiagnostic(test, expected);

			var fixtest = @"
namespace ConsoleApplication1
{
	class TypeName
	{   
		static void Main(string[] args)
		{
			DateTime date = DateTime.UtcNow;
		}
	}
}";

			//VerifyCSharpFix(test, fixtest, null, true);
		}

		[TestMethod]
		public void DateTimeAnalyzer_InitializeDateTimeLocal_ProposeFix()
		{
			var test = @"
namespace ConsoleApplication1
{
	class TypeName
	{   
		static void Main(string[] args)
		{
			DateTime dateTime = new DateTime(0, 0, 0, 0, 0, 0, DateTimeKind.Local);
		}
	}
}";

			var expected = new DiagnosticResult
			{
				Id = "DateTimeUTC",
				Message = String.Format("Can be made DateTime.UtcNow"),
				Severity = DiagnosticSeverity.Warning,
				Locations =
					new[] {
							new DiagnosticResultLocation("Test0.cs", 8,24)
						}
			};

			VerifyCSharpDiagnostic(test, expected);

			var fixtest = @"
namespace ConsoleApplication1
{
	class TypeName
	{   
		static void Main(string[] args)
		{
			DateTime dateTime = new DateTime(0, 0, 0, 0, 0, 0, DateTimeKind.Utc);
		}
	}
}";

			//VerifyCSharpFix(test, fixtest, null, true);
		}

		protected override CodeFixProvider GetCSharpCodeFixProvider()
		{
			return new ConstantFixProvider();
		}

		protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
		{
			return new DateTimeAnalyzer();
		}


		[TestMethod]
		public void DateTimeAnalyzer_DateTimeNowMethodParameter_ProposeFix()
		{
			var test = @"
namespace ConsoleApplication1
{
	class TypeName
	{   
		static void Main(string[] args)
		{
			MyMethod(DateTime.Now);
		}

		private static MyMethod(DateTime dateTime)
		{
		}
	}
}";

			var expected = new DiagnosticResult
			{
				Id = "DateTimeUTC",
				Message = String.Format("Can be made DateTime.UtcNow"),
				Severity = DiagnosticSeverity.Warning,
				Locations =
					new[] {
							new DiagnosticResultLocation("Test0.cs", 8,13)
						}
			};

			VerifyCSharpDiagnostic(test, expected);
		}
	}
}
