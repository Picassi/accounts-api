using System;
using System.Collections.Generic;
using System.Linq;

namespace Picassi.Core.Accounts.Time
{
    public class DateUtilities
    {
        public static IEnumerable<DateTime> GetAllMatchingDaysOfTheWeek(DateTime start, DateTime end, DayOfWeek dayOfWeek)
        {
            var numDaysInRange = (end - start).TotalDays;
            return Enumerable.Range(0, (int)numDaysInRange).Select(day => start.AddDays(day)).Where(day => day.DayOfWeek == dayOfWeek);
        }

        public static IEnumerable<DateTime> GetAllMatchingDaysOfTheMonth(DateTime start, DateTime end, int dayOfMonth)
        {
            var numDaysInRange = (end - start).TotalDays;
            return Enumerable.Range(0, (int)numDaysInRange).Select(day => start.AddDays(day)).Where(day => day.Day == dayOfMonth);
        }
    }
}
