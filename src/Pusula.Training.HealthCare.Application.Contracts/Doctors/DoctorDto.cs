using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Pusula.Training.HealthCare.Doctors
{
    public class DoctorDto : FullAuditedEntityDto<Guid>
    {
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
