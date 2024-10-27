using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Pusula.Training.HealthCare.Leaves;

public class LeaveDto : FullAuditedEntityDto<Guid>
{
    
    [Required]
    public Guid EmployeeId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public string LeaveType { get; set; } = null!;

    [Required]
    public string Status { get; set; } = null!;
    
}