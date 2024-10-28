using System;
using System.ComponentModel.DataAnnotations;

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
    public string LeaveType { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = null!;

}