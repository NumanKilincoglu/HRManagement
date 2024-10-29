using System;
using Volo.Abp.Domain.Entities.Events.Distributed;

namespace Pusula.Training.HealthCare.Employees;

public class EmployeeViewedEto : EtoBase
{
    public Guid Id { get; set; }

    public DateTime ViewedAt { get; set; }
}