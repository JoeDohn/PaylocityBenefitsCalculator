using Api.Models;

namespace Api.Dtos.Employee;

public class GetPaycheckDto
{
    public int EmployeeId { get; set; }

    public DateInterval PaycheckPeriod { get; set; }

    public decimal GrossSalary { get; set; }

    public decimal Benefits { get; set; }

    public decimal NetSalary { get; set; }
}
