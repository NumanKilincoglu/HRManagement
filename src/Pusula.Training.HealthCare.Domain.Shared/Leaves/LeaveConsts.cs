namespace Pusula.Training.HealthCare.Leaves;

public class LeaveConsts
{
    private const string DefaultSorting = "{0}StartDate asc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Leave." : string.Empty);
    }

    public const int FirstNameMaxLength = 128;
    public const int LastNameMaxLength = 128;
    public const int FirstNameMinLength = 2;
    public const int LastNameMinLength = 2;
    public const int IdentityNumberMaxLength = 11;
    public const int EmailAddressMaxLength = 128;
    public const int MobilePhoneNumberMaxLength = 32;

    
}