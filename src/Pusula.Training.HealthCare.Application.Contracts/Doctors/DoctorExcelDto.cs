using System;

namespace Pusula.Training.HealthCare.Doctors
{
    public class DoctorExcelDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public EnumGender Gender { get; set; }
        public string Department { get; set; } = null!;
        public string Title { get; set; } = null!;
    }
}
