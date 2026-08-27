using System;
using TwoWaySync.Services;
using Xunit;

namespace TwoWaySync.Tests
{
    public class TimeZoneHelperTests
    {
        [Fact]
        public void ConvertToUtc_ReturnsCetTime()
        {
            var date = new DateTime(2026, 2, 5, 12, 0, 0, DateTimeKind.Utc);
            var result = TimeZoneHelper.ConvertToUtc(date);
            
            Assert.Equal(new DateTime(2026, 2, 5, 11, 0, 0, DateTimeKind.Utc), result);
        }
    }
}