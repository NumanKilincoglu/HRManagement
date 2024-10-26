using System;
using Pusula.Training.HealthCare.Employees;

namespace Pusula.Training.HealthCare.Employees
{
    public class EmployeeExcelDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public EnumGender Gender { get; set; }
        
        public double Salary { get; set; }
    }
}
