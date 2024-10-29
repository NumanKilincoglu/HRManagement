using System;
using Volo.Abp.Domain.Entities.Events.Distributed;

namespace Pusula.Training.HealthCare.Leaves;

public class LeaveDeletedEto : EtoBase
{
    public Guid Id { get; set; }

    public DateTime DeletedAt { get; set; }
}