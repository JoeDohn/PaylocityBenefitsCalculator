﻿using Api.Dtos.Dependent;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IDependentService _dependentService;

    public DependentsController(IDependentService dependentService)
    {
        _dependentService = dependentService;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = await _dependentService.GetDependentById(id);

        var result = new ApiResponse<GetDependentDto>
        {
            Data = dependent,
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<IEnumerable<GetDependentDto>>>> GetAll()
    {
        var dependents = await _dependentService.GetAllDependents();

        var result = new ApiResponse<IEnumerable<GetDependentDto>>
        {
            Data = dependents,
            Success = true
        };

        return result;
    }
}
