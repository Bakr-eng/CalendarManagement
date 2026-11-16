using System;
using System.Collections.Generic;
using System.Text;
using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Interfaces;
public interface IActivityRepository
{
    Task<List<Activity>> GetAllAsync();
    Task<Activity> AddAsync(Activity activity);
}

