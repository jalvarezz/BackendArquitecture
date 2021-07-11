﻿using System;

namespace Core.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime WithWholeDay(this DateTime obj)
        {
            return obj.Date.AddHours(DateTime.Now.Hour - 23).AddMinutes(DateTime.Now.Minute - 59).AddSeconds(DateTime.Now.Second - 59).AddDays(1);
        }
    }
}
