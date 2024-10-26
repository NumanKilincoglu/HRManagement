using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Pusula.Training.HealthCare.Employees;

public class Leave : FullAuditedAggregateRoot<Guid>
{
    public Guid EmployeeId { get; set; }

    [NotNull]
    public DateTime StartDate { get; set; }

    [NotNull]
    public DateTime EndDate { get; set; }

    [NotNull]
    public string LeaveType { get; set; } = null!;

    [NotNull]
    public string Status { get; set; } = null!;
    
    protected Leave()
    {
        EmployeeId = Guid.Empty;
        StartDate = DateTime.Now;
        EndDate = DateTime.Now;
        LeaveType = string.Empty;
        Status = string.Empty;
    }

    public Leave(Guid id, Guid employeeId, DateTime startDate, DateTime endDate, string leaveType, string status)
    {
        
        Check.NotNull(employeeId, nameof(employeeId));
        Check.NotNull(startDate, nameof(startDate));
        Check.NotNull(endDate, nameof(endDate));
        Check.NotNull(leaveType, nameof(leaveType));
        Check.NotNull(status, nameof(status));
        
        Id = id;
        EmployeeId = employeeId;
        StartDate = startDate;
        EndDate = endDate;
        LeaveType = leaveType;
        Status = status;
    }
}