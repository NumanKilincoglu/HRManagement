using System.ComponentModel.DataAnnotations;
using System;

namespace Pusula.Training.HealthCare.Doctors
{
    public class DoctorUpdateDto
    {
        [Required]
        public Guid Id { get; set; } = default!;
        [Required]
        [StringLength(DoctorConsts.FirstNameMaxLength, MinimumLength = DoctorConsts.FirstNameMinLength)]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(DoctorConsts.LastNameMaxLength, MinimumLength = DoctorConsts.LastNameMinLength)]
        public string LastName { get; set; } = null!;
        [Required]
        [StringLength(DoctorConsts.PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public DateTime BirthDate { get; set; }
        public EnumGender Gender { get; set; }

        public Guid DepartmentId { get; set; }
        public Guid TitleId { get; set; }
    }
}
