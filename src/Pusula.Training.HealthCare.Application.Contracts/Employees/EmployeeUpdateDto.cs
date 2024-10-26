using System.ComponentModel.DataAnnotations;
using System;

namespace Pusula.Training.HealthCare.Employees
{
    public class EmployeeUpdateDto
    {
        [Required]
        public Guid Id { get; set; } = default!;
        
        [Required]
        [StringLength(EmployeeConsts.FirstNameMaxLength, MinimumLength = EmployeeConsts.FirstNameMinLength)]
        public string FirstName { get; set; } = null!;
        
        [Required]
        [StringLength(EmployeeConsts.LastNameMaxLength, MinimumLength = EmployeeConsts.LastNameMinLength)]
        public string LastName { get; set; } = null!;
        
        [Required]
        [StringLength(EmployeeConsts.IdentityNumberMaxLength)]
        public string IdentityNumber { get; set; } = null!;
        
        [Required]
        [StringLength(EmployeeConsts.EmailAddressMaxLength)]
        public string EmailAddress { get; set; } = null!;
        
        [Required]
        [StringLength(EmployeeConsts.PhoneNumberMaxLength)]
        public string MobilePhoneNumber { get; set; } = null!;
        
        [StringLength(EmployeeConsts.PhoneNumberMaxLength)]
        public string HomePhoneNumber { get; set; } = null!;
        
        [StringLength(EmployeeConsts.SalaryMaxLength, MinimumLength = EmployeeConsts.SalaryMinLength)]
        public double Salary { get; set; } = 0.0!;
        
        public DateTime BirthDate { get; set; }
        public EnumGender Gender { get; set; }
    }
}
