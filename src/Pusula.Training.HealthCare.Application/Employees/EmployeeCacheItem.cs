using System;

namespace Pusula.Training.HealthCare.Employees;

public class EmployeeCacheItem
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string IdentityNumber { get; set; } = null!;
    public string EmailAddress { get; set; } = null!;
    public string MobilePhoneNumber { get; set; } = null!;
    public string HomePhoneNumber { get; set; } = null!;
    public string Salary { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public EnumGender Gender { get; set; }

}