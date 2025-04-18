﻿using Api.Dtos.Employee;

namespace Api.Services
{
    public interface IEmployeeService
    {
        Task<GetEmployeeDto> GetEmployeeById(int id);

        Task<IEnumerable<GetEmployeeDto>> GetAllEmployees();

        Task<GetPaycheckDto> CalculatePaycheck(int id);
    }
}
