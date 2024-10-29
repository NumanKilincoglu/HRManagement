using System;
using Pusula.Training.HealthCare.Employees;

namespace Pusula.Training.HealthCare.Employees

{
    public class EmployeeExcelDownloadDto
    {
        public string DownloadToken { get; set; } = null!;
        public string? FilterText { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDateMin { get; set; }
        public DateTime? BirthDateMax { get; set; }
        public int? Gender { get; set; }
        
        public EmployeeExcelDownloadDto() { }
    }
}
