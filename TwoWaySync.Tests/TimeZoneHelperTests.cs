using System;
using TwoWaySync.Services;
using Xunit;

namespace TwoWaySync.Tests
{
    public class TimeZoneHelperTests
    {
        [Theory]
        [InlineData(2026, 2, 5, 12, 11)]
        [InlineData(2026, 6, 30, 20, 18)]
        public void ConvertToUtc_ReturnsUtcTime(int year, int month, int day, int sweHour, int expectedReturnHour)
        {
            var date = new DateTime(year, month, day, sweHour, 0, 0, DateTimeKind.Unspecified);
            var result = TimeZoneHelper.ConvertToUtc(date);
            
            Assert.Equal(new DateTime(year, month, day, expectedReturnHour, 0, 0), result);
        }

        [Theory]
        [InlineData(2026, 2, 5, 12, 13)]
        [InlineData(2026, 6, 30, 16, 18)]
        public void ConvertToSwedishTime_ReturnsSwedishTime(int year, int month, int day, int utcHour, int expectedReturnHour)
        {
            var date = new DateTime(year, month, day, utcHour, 0, 0, DateTimeKind.Utc);
            var result = TimeZoneHelper.ConvertToSwedishTime(date);
            
            Assert.Equal(new DateTime(year, month, day, expectedReturnHour, 0, 0), result);
        }
    }
}