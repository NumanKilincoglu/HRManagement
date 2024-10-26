using Pusula.Training.HealthCare.Employees;

namespace Pusula.Training.HealthCare.Employees
{
    public class EmployeeWithNavigationPropertiesDto
    {
        public EmployeeDto Employee { get; set; } = null!;
        public LeaveDto Leave { get; set; } = null!;
    }
}
