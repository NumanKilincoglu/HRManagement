using Pusula.Training.HealthCare.Leaves;

namespace Pusula.Training.HealthCare.Employees;

public class EmployeeWithNavigationProperties
{
    public Employee Employee { get; set; } = null!;
    public Leave Leave { get; set; } = null!;
}