using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
    class LinqTests
    {
        public void TestMethod()
        {
            var list = new HashSet<string>();
            var result1 = list.Any(e => e == "test");
            var result2 = list.Any(e => e == "test" && e == "othertest");
            var result3 = list.Any(e => "test" == e);

            var collection = new HashSet<int>();
            var result4 = collection.Any(e => e < 2);
            var result5 = collection.Any(e => e > 2);
            var result6 = collection.Any(e => 5 == e);
            var result7 = collection.Any(e => e == 5);
        }
    }
}
