using System;
using System.Collections.Generic;
using System.Text;
using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Interfaces;
public interface IActivityRepository
{
    Task<List<Activity>> GetAllAsync(
        CancellationToken ct = default);
    Task<Activity> AddAsync(
        Activity activity,
        CancellationToken ct = default);
    Task<Activity?> GetByIdAsync(
        int id,
        CancellationToken ct = default);
}

