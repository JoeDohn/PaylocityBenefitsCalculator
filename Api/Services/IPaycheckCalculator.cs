using Api.Models;

namespace Api.Services
{
    public interface IPaycheckCalculator
    {
        decimal CalculatePaycheck(Employee employee);
    }
}
