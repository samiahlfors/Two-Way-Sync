using System;

namespace TwoWaySync.Services
{
    // https://learn.microsoft.com/en-us/dotnet/standard/datetime/converting-between-time-zones
    public static class TimeZoneHelper
    {
        private static TimeZoneInfo SweTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

        public static DateTime ConvertToUtc(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, SweTimeZone);
        }
    }
}