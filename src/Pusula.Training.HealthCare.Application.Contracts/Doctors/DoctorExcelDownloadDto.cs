using System;

namespace Pusula.Training.HealthCare.Doctors
{
    public class DoctorExcelDownloadDto
    {
        public string DownloadToken { get; set; } = null!;
        public string? FilterText { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDateMin { get; set; }
        public DateTime? BirthDateMax { get; set; }
        public EnumGender? Gender { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? TitleId { get; set; }


        public DoctorExcelDownloadDto() { }
    }
}
