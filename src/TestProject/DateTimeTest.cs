using System;
using System.Collections.Generic;
using System.Text;

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
		}

		private void MyMethod(DateTime dateTime)
		{
		}

	}
}
