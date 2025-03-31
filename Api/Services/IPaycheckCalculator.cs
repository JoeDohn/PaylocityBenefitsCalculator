using Api.Dtos.Employee;
using Api.Models;

namespace Api.Services
{
    public interface IPaycheckCalculator
    {
        GetPaycheckDto CalculatePaycheck(Employee employee);
    }
}
