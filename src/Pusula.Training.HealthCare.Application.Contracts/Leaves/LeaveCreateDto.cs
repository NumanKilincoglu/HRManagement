using System;
using System.ComponentModel.DataAnnotations;
using Pusula.Training.HealthCare.Employees;

namespace Pusula.Training.HealthCare.Leaves;

public class LeaveCreateDto
{
    
    [Required]
    public Guid EmployeeId { get; set; }

    [Required]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Required]
    public DateTime EndDate { get; set; } = DateTime.Now;

    [Required]
    [StringLength(LeaveConsts.LeaveTypeMaxLength, MinimumLength = LeaveConsts.LeaveTypeMinLength)]
    public string LeaveType { get; set; } = string.Empty;

    [StringLength(LeaveConsts.LeaveStatusMaxLength, MinimumLength = LeaveConsts.LeaveStatusMinLength)]
    public string Status { get; set; } = null!;

}