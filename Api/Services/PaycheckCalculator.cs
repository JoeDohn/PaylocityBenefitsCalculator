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

        public decimal CalculatePaycheck(Employee employee)
        {
            var currentDateTime = DateTime.UtcNow;
            var annualGrossSalary = employee.Salary;

            var annualBaseCost = _settings.BaseEmployeeCost * 12;
            var annualDependentCost = employee.Dependents.Sum(x => CalcaulateDependentCost(x, currentDateTime)) * 12;
            var annualHighEarnerCost = annualGrossSalary > _settings.HighEarnerThreshold
                ? (annualGrossSalary * _settings.HighEarnerPercentage / 100)
                : 0;

            var totalAnnualDeductions = annualBaseCost + annualDependentCost + annualHighEarnerCost;
            var annualNetSalary = annualGrossSalary - totalAnnualDeductions;
            // Since we pay in USD we can have only 2 decimals (cents)
            var perPaycheckNetSalary = Math.Round(annualNetSalary / _settings.PaychecksPerYear, 2);

            return perPaycheckNetSalary;
        }

        private decimal CalcaulateDependentCost(Dependent dependent, DateTime currentDateTime)
        {
            var currentInterval = _yearIntervalProvider.GetCurrentInterval(currentDateTime, _settings.PaychecksPerYear);

            // I decided to calculate this discount for the whole period.
            // It means, if dependent age is less then 50 years on the moment of start date of current period, we will not give any discount.
            // Some of the requirements are not clear, so I don't want to implement complex solution before clarification.
            var isSenior = dependent.DateOfBirth.AddYears(_settings.SeniorDependentAgeThreshold) <= currentInterval.StartDate;

            return isSenior
                ? _settings.SeniorDependentCost
                : _settings.DependentCost;
        }
    }
}
