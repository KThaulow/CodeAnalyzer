using System;
using System.Globalization;

namespace TestProject
{
	class Program
	{
		static void Main(string[] args)
		{
			int i = 1;
			int j = 2;
			int k = i + j;

			var dateTime = DateTime.Now;

			var dateTime2 = new DateTime(0, 0, 0, 0, 0, 0, DateTimeKind.Local);

			MyMethod(DateTime.Now);

			double parsedDouble = double.Parse("1.1", NumberStyles.Any, CultureInfo.InvariantCulture);

			double parsedDouble2 = double.Parse("1.1");

			bool didParse = double.TryParse("1.1", NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedDouble3);

			bool didParse2 = double.TryParse("1.1", out double parsedDouble4);

			int i = 0;
			while (i < 10)
			{
				i--;
				i++;
				return;
			}
		}


		private static void MyMethod(DateTime dateTime)
		{
		}
	}
}
