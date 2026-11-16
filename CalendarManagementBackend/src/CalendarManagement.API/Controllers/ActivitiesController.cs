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
        if (!dto.Date.HasValue)
        {
            return BadRequest("Date is required.");
        }

        var activityType = AllowedTypes.FirstOrDefault(
            type => string.Equals(
                type,
                dto.Type?.Trim(),
                StringComparison.OrdinalIgnoreCase
            )
        );

        if (activityType == null)
        {
            return BadRequest("Invalid activity type.");
        }

        var activity = new Activity
        {
            Date = dto.Date.Value,
            Type = activityType
        };

        var createdActivity = await _repository.AddAsync(activity);

        return StatusCode(201, createdActivity);
    }
    private static readonly string[] AllowedTypes =
    {
    "Gig",
    "Private",
    "Rehearsal",
    "Vacation",
    "Travel Day",
    "Pre-production",
    "Individual Work"
};

}
