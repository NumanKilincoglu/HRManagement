using System;
using Volo.Abp.BackgroundJobs;

namespace Pusula.Training.HealthCare.Leaves;

[BackgroundJobName("leave-view-log")]
public class LeaveViewLogArgs
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
}