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
    
        [Required]
        [Range(EmployeeConsts.SalaryMin, EmployeeConsts.SalaryMax)]
        public double Salary { get; set; } = 0!;
    
        [Required]
        public DateTime BirthDate { get; set; }

        [Range(EmployeeConsts.GenderMinLength, EmployeeConsts.GenderMaxLength)]
        public int Gender { get; set; }
    }
}
