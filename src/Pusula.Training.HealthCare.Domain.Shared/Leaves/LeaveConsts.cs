namespace Pusula.Training.HealthCare.Leaves;

public class LeaveConsts
{
    private const string DefaultSorting = "{0}StartDate asc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Leave." : string.Empty);
    }
    public const int LeaveTypeMaxLength = 50;
    public const int LeaveStatusMaxLength = 50;
    public const int LeaveTypeMinLength = 2;
    public const int LeaveStatusMinLength = 2;
}