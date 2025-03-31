using Api.Models;

namespace Api.Services
{
    // Need this class to calculate dates of current period. Will use it for applying senior discount
    public class YearIntervalProvider : IYearIntervalProvider
    {
        public DateInterval GetCurrentInterval(DateTime currentDate, int numberOfIntervals)
        {
            var yearIntervals = GetYearIntervals(currentDate.Year, numberOfIntervals);

            return yearIntervals.Single(x => x.StartDate <= currentDate.Date && x.EndDate >= currentDate.Date);
        }

        private static IEnumerable<DateInterval> GetYearIntervals(int year, int numberOfIntervals)
        {
            var intervals = new List<DateInterval>();

            var totalDays = DateTime.IsLeapYear(year) ? 366 : 365;

            // Calculate it for calendar days.
            // We can improve it by calculating intervals for working days and trying to make them as evenly as possible,
            // but from current requirements it's not clear if I need to make it more complex.
            var intervalDays = totalDays / numberOfIntervals;
            var remainingDays = totalDays % numberOfIntervals; // Remaining days to be distributed

            var startDate = new DateTime(year, 1, 1);
            var step = remainingDays > 0 ? numberOfIntervals / remainingDays : 0;  // Calculate step for distributing remaining days

            for (var i = 0; i < numberOfIntervals; i++)
            {
                // Initial number of days in the interval
                var daysInInterval = intervalDays;

                // Distribute the remaining days through the calculated step
                if (remainingDays > 0 && i % step == 0)
                {
                    daysInInterval++; // Add an extra day
                    remainingDays--;  // Decrease the number of remaining days
                }

                var endDate = startDate.AddDays(daysInInterval - 1);
                intervals.Add(new DateInterval { StartDate = startDate, EndDate = endDate });

                startDate = endDate.AddDays(1); // The next interval starts the next day
            }

            return intervals;
        }
    }
}
