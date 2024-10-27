using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Pusula.Training.HealthCare.Employees;

public class EmployeeDto : FullAuditedEntityDto<Guid>
{
    [Required]
    [StringLength(EmployeeConsts.FirstNameMaxLength, MinimumLength = EmployeeConsts.FirstNameMinLength)]
    public string FirstName { get; set; } = null!;
    [Required]
    [StringLength(EmployeeConsts.LastNameMaxLength, MinimumLength = EmployeeConsts.LastNameMinLength)]
    public string LastName { get; set; } = null!;
    [Required]
    [StringLength(EmployeeConsts.PhoneNumberMaxLength)]
    public string MobilePhoneNumber { get; set; } = null!;
    
    [Required]
    public string Salary { get; set; } = null!;
    
    [Required]
    public DateTime BirthDate { get; set; }
    public EnumGender Gender { get; set; }
    
}