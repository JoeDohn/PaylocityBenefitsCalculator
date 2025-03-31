using Api.Dtos.Employee;
using Api.Models;
using Api.Options;
using Microsoft.Extensions.Options;

namespace Api.Services
{
    public class PaycheckCalculator : IPaycheckCalculator
    {
        private readonly IYearIntervalProvider _yearIntervalProvider;
        private readonly PaycheckSettings _settings;

        public PaycheckCalculator(IYearIntervalProvider yearIntervalProvider, IOptions<PaycheckSettings> settings)
        {
            _yearIntervalProvider = yearIntervalProvider;
            _settings = settings.Value;
        }

        public GetPaycheckDto CalculatePaycheck(Employee employee)
        {
            var currentDateTime = DateTime.UtcNow;

            var currentPaycheckPeriod = _yearIntervalProvider.GetCurrentInterval(currentDateTime, _settings.PaychecksPerYear);

            // Since we pay in USD we can have only 2 decimals (cents)
            var perPaycheckGrossSalary = Math.Round(employee.Salary / _settings.PaychecksPerYear, 2);
            var perPaycheckBaseCost = _settings.BaseEmployeeCost * 12 / _settings.PaychecksPerYear;
            var perPaycheckDependentCost = employee.Dependents.Sum(x => CalculatePerPaycheckDependentCost(x, currentDateTime, currentPaycheckPeriod.StartDate));
            var perPaycheckHighEarnerCost = employee.Salary > _settings.HighEarnerThreshold
                ? (perPaycheckGrossSalary * _settings.HighEarnerPercentage / 100)
                : 0;

            var perPaycheckDeductions = Math.Round(perPaycheckBaseCost + perPaycheckDependentCost + perPaycheckHighEarnerCost, 2);
            var perPaycheckNetSalary = perPaycheckGrossSalary - perPaycheckDeductions;

            return new GetPaycheckDto
            {
                EmployeeId = employee.Id,
                Benefits = perPaycheckDeductions,
                GrossSalary = perPaycheckGrossSalary,
                NetSalary = perPaycheckNetSalary,
                PaycheckPeriod = currentPaycheckPeriod
            };
        }

        private decimal CalculatePerPaycheckDependentCost(Dependent dependent, DateTime currentDateTime, DateTime currentPaycheckPeriodStartDate)
        {
            // I decided to calculate this discount for the whole period.
            // It means, if dependent age is less then 50 years on the moment of start date of current period, we will not give senior discount.
            // Some of the requirements are not clear, so I don't want to implement complex solution before clarification.
            var isSenior = dependent.DateOfBirth.AddYears(_settings.SeniorDependentAgeThreshold) <= currentPaycheckPeriodStartDate;

            var monthlyDependentCost = isSenior ? _settings.SeniorDependentCost : _settings.DependentCost;

            return monthlyDependentCost * 12 / _settings.PaychecksPerYear;
        }
    }
}
