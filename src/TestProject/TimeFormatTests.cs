using System;
using System.Globalization;

namespace TestProject
{
    public class TimeFormatTests
    {
        public void Test()
        {
            var time2 = new TimeSpan(20, 10, 5, 2).ToString(@"y", CultureInfo.InvariantCulture);

            var time3 = new TimeSpan(20, 10, 5, 2).ToString(@"Y", CultureInfo.InvariantCulture);

            var time4 = new TimeSpan(20, 10, 5, 2).ToString(@"yy", CultureInfo.InvariantCulture);

            var time5 = new TimeSpan(20, 10, 5, 2).ToString(@"yyy", CultureInfo.InvariantCulture);

            var time6 = new TimeSpan(20, 10, 5, 2).ToString(@"yyyy", CultureInfo.InvariantCulture);

            var time7 = new TimeSpan(20, 10, 5, 2).ToString(@"M", CultureInfo.InvariantCulture);

            var time8 = new TimeSpan(20, 10, 5, 2).ToString(@"MM", CultureInfo.InvariantCulture);

            var time9 = new TimeSpan(20, 10, 5, 2).ToString(@"MMM", CultureInfo.InvariantCulture);

            var time10 = new TimeSpan(20, 10, 5, 2).ToString(@"MMMM", CultureInfo.InvariantCulture);

            var time11 = new TimeSpan(20, 10, 5, 2).ToString(@"HH", CultureInfo.InvariantCulture);

            var time12 = new TimeSpan(20, 10, 5, 2).ToString(@"D", CultureInfo.InvariantCulture);

            var time13 = new TimeSpan(20, 10, 5, 2).ToString(@"DD", CultureInfo.InvariantCulture);

            var time14 = new TimeSpan(20, 10, 5, 2).ToString(@"d", CultureInfo.InvariantCulture);

            var time15 = new TimeSpan(20, 10, 5, 2).ToString(@"h", CultureInfo.InvariantCulture);

            var time16 = new TimeSpan(20, 10, 5, 2).ToString(@"m", CultureInfo.InvariantCulture);

            var time17 = new TimeSpan(20, 10, 5, 2).ToString(@"s", CultureInfo.InvariantCulture);

            var time18 = new TimeSpan(20, 10, 5, 2).ToString(@"f", CultureInfo.InvariantCulture);

            var time19 = new TimeSpan(20, 10, 5, 2).ToString(@"F", CultureInfo.InvariantCulture);




            var time20 = new TimeSpan(20, 10, 5, 2).ToString(@"%d", CultureInfo.InvariantCulture);

            var time21 = new TimeSpan(20, 10, 5, 2).ToString(@"%h", CultureInfo.InvariantCulture);

            var time22 = new TimeSpan(20, 10, 5, 2).ToString(@"%m", CultureInfo.InvariantCulture);

            var time23 = new TimeSpan(20, 10, 5, 2).ToString(@"%s", CultureInfo.InvariantCulture);

            var time24 = new TimeSpan(20, 10, 5, 2).ToString(@"%f", CultureInfo.InvariantCulture);

            var time25 = new TimeSpan(20, 10, 5, 2).ToString(@"%F", CultureInfo.InvariantCulture);


            var time26 = new TimeSpan(20, 10, 5, 2).ToString(@"d\:h", CultureInfo.InvariantCulture);

            var time27 = new TimeSpan(20, 10, 5, 2).ToString(@"h\:m", CultureInfo.InvariantCulture);

            var time28 = new TimeSpan(20, 10, 5, 2).ToString(@"m\:s", CultureInfo.InvariantCulture);

            var time29 = new TimeSpan(20, 10, 5, 2).ToString(@"s\:f", CultureInfo.InvariantCulture);

            var time30 = new TimeSpan(20, 10, 5, 2).ToString(@"f\:ff", CultureInfo.InvariantCulture);

            var time31 = new TimeSpan(20, 10, 5, 2).ToString(@"F\:ff", CultureInfo.InvariantCulture);

        }
    }
}
