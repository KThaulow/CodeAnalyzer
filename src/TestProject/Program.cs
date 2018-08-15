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

			var str = nameof(args);

			double parsedDouble = double.Parse("1.1", NumberStyles.Any, CultureInfo.InvariantCulture);

			double parsedDouble2 = double.Parse("1.1");

			bool didParse = double.TryParse("1.1", NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedDouble3);

			bool didParse2 = double.TryParse("1.1", out double parsedDouble4);

			int l = 0;
			while (i < 10)
			{
				l--;
				l++;
				return;
			}

			while (i < 10)
			{
				l--;
				l++;
			}

		}



	}
}
