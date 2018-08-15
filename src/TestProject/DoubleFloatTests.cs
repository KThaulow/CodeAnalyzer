using System.Globalization;

namespace TestProject
{
	public class DoubleFloatTests
	{
		public void TestMethod()
		{
			double parsedDouble = double.Parse("1.1", NumberStyles.Any, CultureInfo.InvariantCulture);

			double parsedDouble2 = double.Parse("1.1");


			bool didParse = double.TryParse("1.1", NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedDouble3);

			bool didParse2 = double.TryParse("1.1", out double parsedDouble4);

			bool didParse3 = float.TryParse("1.1", out float parsedFloat);

			float parsed1 = float.Parse("1.1", NumberStyles.Any, CultureInfo.InvariantCulture);

			float parsed2 = float.Parse("1.1");

			bool didParse4 = float.TryParse("1.1", out float parsedFloat1);
		}
	}
}
