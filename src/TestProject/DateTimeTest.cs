using System;

namespace TestProject
{
	public class DateTimeTest
	{
		public void TestMethod()
		{
			var dateTime = DateTime.Now;

			var dateTime2 = new DateTime(0, 0, 0, 0, 0, 0, DateTimeKind.Local);

			var dateTime3 = new DateTime(0);

			var dateTime4 = new DateTime(0, 0, 0);

			var dateTime5 = new DateTime(0, 0, 0, 0, 0, 0);

			MyMethod(DateTime.Now);

			string date = DateTime.Now.ToString("mm/dd/yyyy HH:mm:ss");

			string date2 = DateTime.Now.ToString("mm/dd/yyyy hh:mm:ss");

			string dateTimeOffset = new DateTimeOffset(0, new TimeSpan(0)).ToString("mm/dd/yyyy hh:mm:ss");

			var timeSpan = new TimeSpan(0).ToString("mm/dd/yyyy hh:mm:ss");


			var date3 = DateTime.UtcNow.ToString("yyyy/mm/dd");

			string dateTimeOffset2 = DateTimeOffset.Now.ToString("yyyy/mm/dd");

		}

		private void MyMethod(DateTime dateTime)
		{
		}

	}
}
