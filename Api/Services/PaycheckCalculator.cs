using Api.Models;
using Api.Options;
using Microsoft.Extensions.Options;

namespace Api.Services
{
    public class PaycheckCalculator : IPaycheckCalculator
    {
        private readonly PayrollSettings _settings;

        public PaycheckCalculator(IOptions<PayrollSettings> settings)
        {
            _settings = settings.Value;
        }

        public decimal CalculatePaycheck(Employee employee)
        {
            var annualGrossSalary = employee.Salary;

            var annualBaseCost = _settings.BaseEmployeeCost * 12;
            var annualDependentCost = employee.Dependents.Sum(CalcaulateDependentCost) * 12;
            var annualHighEarnerCost = annualGrossSalary > _settings.HighEarnerThreshold
                ? (annualGrossSalary * _settings.HighEarnerPercentage / 100)
                : 0;

            var totalAnnualDeductions = annualBaseCost + annualDependentCost + annualHighEarnerCost;
            var annualNetSalary = annualGrossSalary - totalAnnualDeductions;
            // Since we pay in USD we can have only 2 decimals (cents)
            var perPaycheckNetSalary = Math.Round(annualNetSalary / 26, 2);

            return perPaycheckNetSalary;
        }

        private decimal CalcaulateDependentCost(Dependent dependent)
        {
            var dependentAge = DateTime.Now.Year - dependent.DateOfBirth.Year;

            return dependentAge > _settings.SeniorDependentAgeThreshold
                ? _settings.SeniorDependentCost
                : _settings.DependentCost;
        }
    }
}
