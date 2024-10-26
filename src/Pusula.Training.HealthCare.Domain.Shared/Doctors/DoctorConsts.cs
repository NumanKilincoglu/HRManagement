namespace Pusula.Training.HealthCare.Doctors
{
    public static class DoctorConsts
    {
        private const string DefaultSorting = "{0}FirstName asc";

        public static string GetDefaultSorting(bool withEntityName) => string.Format(DefaultSorting, withEntityName ? "Doctor." : string.Empty);

        public const int FirstNameMinLength = 2;
        public const int FirstNameMaxLength = 50;
        public const int LastNameMinLength = 2;
        public const int LastNameMaxLength = 50;
        public const int PhoneNumberMaxLength = 10;
    }
}
