using Api.Dtos.Employee;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var dependent = await _employeeService.GetEmployeeById(id);

        var result = new ApiResponse<GetEmployeeDto>
        {
            Data = dependent,
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<IEnumerable<GetEmployeeDto>>>> GetAll()
    {
        var dependents = await _employeeService.GetAllEmployees();

        var result = new ApiResponse<IEnumerable<GetEmployeeDto>>
        {
            Data = dependents,
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get employee's paycheck")]
    [HttpGet("{id}/paycheck")]
    public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> GetPaycheck(int id)
    {
        var paycheck = await _employeeService.CalculatePaycheck(id);

        var result = new ApiResponse<GetPaycheckDto>
        {
            Data = paycheck,
            Success = true
        };

        return result;
    }
}
