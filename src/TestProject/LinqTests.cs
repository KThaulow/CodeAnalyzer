using System;
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
            var result1 = list.Any(e => e == "test");
        }
    }
}
