using System;
using Volo.Abp.Application.Dtos;

namespace Pusula.Training.HealthCare.Doctors
{
    public class GetDoctorsInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDateMin { get; set; }
        public DateTime? BirthDateMax { get; set; }
        public EnumGender? Gender { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? TitleId { get; set; }


        public GetDoctorsInput() { }
    }
}
