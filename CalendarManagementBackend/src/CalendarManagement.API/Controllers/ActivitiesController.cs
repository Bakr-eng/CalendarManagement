using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CalendarManagement.Domain.Entities;

namespace CalendarManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly IActivityRepository _repository;

    public ActivitiesController(IActivityRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var activities = await _repository.GetAllAsync();

        return Ok(activities);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(
       [FromBody] CreateActivityDto dto)
    {
        var activity = new Activity
        {
            Date = dto.Date!.Value,
            Type = dto.Type
        };

        var createdActivity = await _repository.AddAsync(activity);

        return StatusCode(201, createdActivity);
    }
}
