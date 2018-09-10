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
            var result2 = list.Any(e => "test" == e);

            var lol = list.Contains("test");

            var list1 = new List<int>();
            var result3 = list1.Any(e => e == 4);

            if (list.Any(e => "test" == e))
            {
                ;
            }
        }
    }
}
