namespace Pusula.Training.HealthCare.Employees;

public class EmployeeConsts
{
    private const string DefaultSorting = "{0}FirstName asc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Employee." : string.Empty);
    }

    public const int FirstNameMaxLength = 128;
    public const int LastNameMaxLength = 128;
    public const int FirstNameMinLength = 2;
    public const int LastNameMinLength = 2;
    public const int IdentityNumberMaxLength = 11;
    public const int EmailAddressMaxLength = 128;
    public const int MobilePhoneNumberMaxLength = 32;
    public const int GenderMinLength = 1;
    public const int GenderMaxLength = 2;
    public const int PhoneNumberMaxLength = 11;
    public const int PhoneNumberMinLength = 11;
    public const double SalaryMin = 0.0;
    public const double SalaryMax = double.MaxValue;
    public const int SalaryMinLength = 1;
    public const int SalaryMaxLength = 9;
    
}