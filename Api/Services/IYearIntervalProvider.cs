
using Api.Models;

namespace Api.Services
{
    public interface IYearIntervalProvider
    {
        DateInterval GetCurrentInterval(DateTime currentDate, int numberOfIntervals);
    }
}