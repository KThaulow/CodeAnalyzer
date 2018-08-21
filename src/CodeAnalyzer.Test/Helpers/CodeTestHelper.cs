using System;
using Microsoft.CodeAnalysis;
using TestHelper;

namespace CodeAnalyzer.Test.Helpers
{
    public class CodeTestHelper
    {

        public static string GetCodeInMainMethod(string nameSpaces, string methodBody)
        {
            var test = @"
" + nameSpaces + @"
namespace ConsoleApplication1
{
	class TypeName
	{   
		static void Main(string[] args)
		{
			" + methodBody + @"
		}
	}
}";

            return test;
        }

        public static DiagnosticResult CreateDiagnosticResult(string id, string message, int lineNum, int columnNum)
        {
            return new DiagnosticResult
            {
                Id = id,
                Message = String.Format(message),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", lineNum,columnNum)
                        }
            };
        }

        public static DiagnosticResult CreateDiagnosticResult(string id, int lineNum, int columnNum)
        {
            return new DiagnosticResult
            {
                Id = id,
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", lineNum,columnNum)
                        }
            };
        }
    }
}
