using System;
using System.Collections.Generic;
using Api.Models;
using Api.Services;
using Xunit;

namespace ApiTests.UnitTests
{
    public class YearIntervalProviderTests
    {
        private readonly YearIntervalProvider _provider = new YearIntervalProvider();

        public static IEnumerable<object[]> NonLeapYearTestData => new List<object[]>
        {
            new object[] { new DateTime(2023, 1, 1), new DateInterval { StartDate = new DateTime(2023, 1, 1), EndDate = new DateTime(2023, 7, 2) } },
            new object[] { new DateTime(2023, 4, 22), new DateInterval { StartDate = new DateTime(2023, 1, 1), EndDate = new DateTime(2023, 7, 2) } },
            new object[] { new DateTime(2023, 7, 2), new DateInterval { StartDate = new DateTime(2023, 1, 1), EndDate = new DateTime(2023, 7, 2) } },
            new object[] { new DateTime(2023, 7, 3), new DateInterval { StartDate = new DateTime(2023, 7, 3), EndDate = new DateTime(2023, 12, 31) } },
            new object[] { new DateTime(2023, 9, 12), new DateInterval { StartDate = new DateTime(2023, 7, 3), EndDate = new DateTime(2023, 12, 31) } },
            new object[] { new DateTime(2023, 12, 31), new DateInterval { StartDate = new DateTime(2023, 7, 3), EndDate = new DateTime(2023, 12, 31) } }
        };

        public static IEnumerable<object[]> LeapYearTestData => new List<object[]>
        {
            new object[] { new DateTime(2020, 1, 1), new DateInterval { StartDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2020, 7, 1) } },
            new object[] { new DateTime(2020, 4, 22), new DateInterval { StartDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2020, 7, 1) } },
            new object[] { new DateTime(2020, 7, 1), new DateInterval { StartDate = new DateTime(2020, 1, 1), EndDate = new DateTime(2020, 7, 1) } },
            new object[] { new DateTime(2020, 7, 2), new DateInterval { StartDate = new DateTime(2020, 7, 2), EndDate = new DateTime(2020, 12, 31) } },
            new object[] { new DateTime(2020, 9, 12), new DateInterval { StartDate = new DateTime(2020, 7, 2), EndDate = new DateTime(2020, 12, 31) } },
            new object[] { new DateTime(2020, 12, 31), new DateInterval { StartDate = new DateTime(2020, 7, 2), EndDate = new DateTime(2020, 12, 31) } }
        };

        [Theory]
        [MemberData(nameof(NonLeapYearTestData))]
        public void GetCurrentInterval_NonLeapYear_ShouldReturnCorrectInterval(DateTime currentDate, DateInterval expectedInterval)
        {
            // Arrange
            int numberOfIntervals = 2;

            // Act
            var interval = _provider.GetCurrentInterval(currentDate, numberOfIntervals);

            // Assert
            Assert.Equal(expectedInterval.StartDate, interval.StartDate);
            Assert.Equal(expectedInterval.EndDate, interval.EndDate);
        }

        [Theory]
        [MemberData(nameof(LeapYearTestData))]
        public void GetCurrentInterval_LeapYear_ShouldReturnCorrectInterval(DateTime currentDate, DateInterval expectedInterval)
        {
            // Arrange
            int numberOfIntervals = 2;

            // Act
            var interval = _provider.GetCurrentInterval(currentDate, numberOfIntervals);

            // Assert
            Assert.Equal(expectedInterval.StartDate, interval.StartDate);
            Assert.Equal(expectedInterval.EndDate, interval.EndDate);
        }
    }
}
