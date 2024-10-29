using System;
using Pusula.Training.HealthCare.Employees;
using Volo.Abp.Application.Dtos;

namespace Pusula.Training.HealthCare.Leaves;

public class GetLeavesInput : PagedAndSortedResultRequestDto
{
    public Guid? EmployeeId { get; set; }
    public string? LeaveType { get; set; }
    public string? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public GetLeavesInput()
    {
    }
}