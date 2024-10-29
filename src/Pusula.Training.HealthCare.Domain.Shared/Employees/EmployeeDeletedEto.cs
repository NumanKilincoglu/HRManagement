using System;
using Volo.Abp.Domain.Entities.Events.Distributed;

namespace Pusula.Training.HealthCare.Employees;

public class EmployeeDeletedEto : EtoBase
{
    public Guid Id { get; set; }

    public DateTime DeletedAt { get; set; }
}