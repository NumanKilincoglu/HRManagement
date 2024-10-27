using System;
using System.ComponentModel.DataAnnotations;

namespace Pusula.Training.HealthCare.Leaves;

public class LeaveCreateDto
{
    
    
    
    [Required]
    public double Salary { get; set; } = 0.0!;
    
    [Required]
    public DateTime BirthDate { get; set; }
    
}