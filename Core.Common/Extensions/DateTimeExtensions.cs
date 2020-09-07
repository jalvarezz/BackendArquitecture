using System;
using System.Globalization;

namespace Core.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime WithWholeDay(this DateTime obj)
        {
            return obj.Date.AddHours(DateTime.Now.Hour - 23).AddMinutes(DateTime.Now.Minute - 59).AddSeconds(DateTime.Now.Second - 59).AddDays(1);
        }

        public static string DateFormatString(DateTime? date)
        {
            
           return date?.ToString("dd/MMM/yyyy");            

           
        }

        public static int CalculateAge(this DateTime birthDate, DateTime ohterDate)
        {
            int age = ohterDate.Year - birthDate.Year;

            if (birthDate > ohterDate.AddYears(-age))
                age--;

            return age;
        }

        public static DateTime GetDateFromWeekNumber(this DateTime date, int weekNumber)
        {
            var firstDayOfyear = new DateTime(date.Year, 1, 1);
            int daysOffset = DayOfWeek.Monday - firstDayOfyear.DayOfWeek;

            DateTime firstMonday = firstDayOfyear.AddDays(daysOffset);

            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstDayOfyear, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekNumber;
            if(firstWeek <= 1)
            {
                weekNum -= 1;
            }

            var result = firstMonday.AddDays((weekNum * 7) + (int)DayOfWeek.Monday - 1);
            return result;
        }
    }
}
