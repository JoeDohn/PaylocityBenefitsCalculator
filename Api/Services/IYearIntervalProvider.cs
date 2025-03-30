
namespace Api.Services
{
    public interface IYearIntervalProvider
    {
        (DateTime StartDate, DateTime EndDate) GetCurrentInterval(DateTime currentDate, int numberOfIntervals);
    }
}