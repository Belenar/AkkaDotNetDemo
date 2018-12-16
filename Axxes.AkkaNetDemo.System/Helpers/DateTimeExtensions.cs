using System;

namespace Axxes.AkkaNetDemo.System.Helpers
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime ReferenceDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static int QuarterNumber(this DateTime date)
        {
            var timespan = date - ReferenceDate;

            return (int) Math.Floor(timespan.TotalMinutes/15);
        }

        public static int HourNumber(this DateTime date)
        {
            var timespan = date - ReferenceDate;

            return (int)Math.Floor(timespan.TotalHours);
        }

        public static DateTime GetQuarterDate(int quarterNumber)
        {
            return ReferenceDate.AddMinutes(quarterNumber * 15);
        }

        public static DateTime GetHourDate(int hourNumber)
        {
            return ReferenceDate.AddHours(hourNumber);
        }
    }
}
