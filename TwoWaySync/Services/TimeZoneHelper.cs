using System;

namespace TwoWaySync.Services
{
    // https://learn.microsoft.com/en-us/dotnet/standard/datetime/converting-between-time-zones
    public static class TimeZoneHelper
    {
        private static readonly TimeZoneInfo SweTimeZone = GetSwedishTimeZone();
        
        private static TimeZoneInfo GetSwedishTimeZone()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Europe/Stockholm");
            }
        }

        public static DateTime ConvertToUtc(DateTime dateTime) => TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified), SweTimeZone);
        public static DateTime ConvertToSwedishTime(DateTime dateTime) => TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc), SweTimeZone);
    }
}