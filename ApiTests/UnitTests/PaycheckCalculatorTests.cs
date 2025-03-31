using System;
using System.Collections.Generic;
using Api.Models;
using Api.Options;
using Api.Services;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ApiTests.UnitTests
{
    public class PaycheckCalculatorTests
    {
        private readonly Mock<IYearIntervalProvider> _yearIntervalProviderMock;
        private readonly PaycheckSettings _settings;
        private readonly PaycheckCalculator _paycheckCalculator;

        public PaycheckCalculatorTests()
        {
            _yearIntervalProviderMock = new Mock<IYearIntervalProvider>();

            _settings = new PaycheckSettings
            {
                PaychecksPerYear = 26,
                BaseEmployeeCost = 1000m,
                DependentCost = 500m,
                SeniorDependentCost = 400m,
                SeniorDependentAgeThreshold = 50,
                HighEarnerThreshold = 80000m,
                HighEarnerPercentage = 2
            };

            _paycheckCalculator = new PaycheckCalculator(_yearIntervalProviderMock.Object, Options.Create(_settings));
        }

        [Fact]
        public void CalculatePaycheck_EmployeeWithRegularBenefits_ShouldReturnPaycheck()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 1,
                Salary = 52000,
                Dependents = new List<Dependent>()
            };

            var paycheckPeriod = new DateInterval { StartDate = new DateTime(2024, 6, 1), EndDate = new DateTime(2024, 6, 14) };

            _yearIntervalProviderMock
                .Setup(p => p.GetCurrentInterval(It.IsAny<DateTime>(), _settings.PaychecksPerYear))
                .Returns(paycheckPeriod);

            // Act
            var result = _paycheckCalculator.CalculatePaycheck(employee);

            // Assert
            var expectedGrossSalary = Math.Round(employee.Salary / _settings.PaychecksPerYear, 2);
            var expectedBaseCost = Math.Round(_settings.BaseEmployeeCost * 12 / _settings.PaychecksPerYear, 2);
            var expectedNetSalary = expectedGrossSalary - expectedBaseCost;

            Assert.Equal(employee.Id, result.EmployeeId);
            Assert.Equal(expectedGrossSalary, result.GrossSalary);
            Assert.Equal(expectedBaseCost, result.Benefits);
            Assert.Equal(expectedNetSalary, result.NetSalary);
            Assert.Equal(paycheckPeriod, result.PaycheckPeriod);
        }

        [Fact]
        public void CalculatePaycheck_EmployeeWithHighEarnerDeduction_ShouldReturnPaycheck()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 2,
                Salary = 90000m, // Above high-earner threshold
                Dependents = new List<Dependent>()
            };

            var paycheckPeriod = new DateInterval { StartDate = new DateTime(2024, 6, 1), EndDate = new DateTime(2024, 6, 14) };

            _yearIntervalProviderMock
                .Setup(p => p.GetCurrentInterval(It.IsAny<DateTime>(), _settings.PaychecksPerYear))
                .Returns(paycheckPeriod);

            // Act
            var result = _paycheckCalculator.CalculatePaycheck(employee);

            // Assert
            var expectedGrossSalary = Math.Round(employee.Salary / _settings.PaychecksPerYear, 2);
            var expectedBaseCost = Math.Round(_settings.BaseEmployeeCost * 12 / _settings.PaychecksPerYear, 2);
            var expectedHighEarnerCost = Math.Round(expectedGrossSalary * _settings.HighEarnerPercentage / 100, 2);
            var expectedTotalDeductions = expectedBaseCost + expectedHighEarnerCost;
            var expectedNetSalary = expectedGrossSalary - expectedTotalDeductions;

            Assert.Equal(expectedGrossSalary, result.GrossSalary);
            Assert.Equal(expectedTotalDeductions, result.Benefits);
            Assert.Equal(expectedNetSalary, result.NetSalary);
        }

        [Fact]
        public void CalculatePaycheck_EmployeeWithDependents_ShouldReturnPaycheck()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 3,
                Salary = 60000m,
                Dependents = new List<Dependent>
                {
                    new Dependent { DateOfBirth = new DateTime(1990, 5, 10) }, // Not a senior
                    new Dependent { DateOfBirth = new DateTime(1960, 8, 22) }  // Senior
                }
            };

            var currentDate = new DateTime(2024, 6, 15);
            var paycheckPeriod = new DateInterval { StartDate = new DateTime(2024, 6, 1), EndDate = new DateTime(2024, 6, 14) };

            _yearIntervalProviderMock
                .Setup(p => p.GetCurrentInterval(It.IsAny<DateTime>(), _settings.PaychecksPerYear))
                .Returns(paycheckPeriod);

            // Act
            var result = _paycheckCalculator.CalculatePaycheck(employee);

            // Assert
            var expectedGrossSalary = Math.Round(employee.Salary / _settings.PaychecksPerYear, 2);
            var expectedBaseCost = Math.Round(_settings.BaseEmployeeCost * 12 / _settings.PaychecksPerYear, 2);
            var expectedDependentCost = Math.Round((_settings.DependentCost * 12 / _settings.PaychecksPerYear) + (_settings.SeniorDependentCost * 12 / _settings.PaychecksPerYear), 2);
            var expectedTotalDeductions = expectedBaseCost + expectedDependentCost;
            var expectedNetSalary = expectedGrossSalary - expectedTotalDeductions;

            Assert.Equal(expectedGrossSalary, result.GrossSalary);
            Assert.Equal(expectedTotalDeductions, result.Benefits);
            Assert.Equal(expectedNetSalary, result.NetSalary);
        }
    }
}
