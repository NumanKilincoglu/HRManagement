using System;
using Pusula.Training.HealthCare.Employees;
using Volo.Abp.Application.Dtos;

namespace Pusula.Training.HealthCare.Employees
{
    public class GetEmployeesInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDateMin { get; set; }
        public DateTime? BirthDateMax { get; set; }
        public int? Gender { get; set; }
        
        public GetEmployeesInput() { }
    }
}
