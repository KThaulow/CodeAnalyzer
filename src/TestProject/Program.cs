using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var timeFormat = new TimeFormatTests();
            timeFormat.Test();

            var list = new List<string>();
            var result = list.Any(e => e == "test");

            if (list.Any(e => e == "test"))
            {
                ;
            }
        }
    }
}
