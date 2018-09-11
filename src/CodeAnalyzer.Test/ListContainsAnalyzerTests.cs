using CodeAnalyzer.Analyzers;
using CodeAnalyzer.Test.Helpers;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace CodeAnalyzer.Test
{
    [TestClass]
    public class ListContainsAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        public void ListContainsAnalyzer_AnyCouldBeContains_ProposeFix()
        {
            var namespaces = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
    ";

            var methodBody = @"
            var list = new List<string>();
            var result = list.Any(e => e == ""test"");
            ";

            //var code = CodeTestHelper.GetCodeInMainMethod(namespaces, methodBody);

            var code = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject
{
    class LinqTests
    {
        public void TestMethod()
        {
            var list = new List<string>();
            var result1 = list.Any(e => e == ""test"");
        }
    }
}
";

            var expected = CodeTestHelper.CreateDiagnosticResult("AN0010", 10, 15);

            VerifyCSharpDiagnostic(code, expected);
        }


        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new ConstantFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new ListContainsAnalyzer();
        }
    }
}
