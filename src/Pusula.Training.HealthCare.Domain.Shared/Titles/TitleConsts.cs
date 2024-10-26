namespace Pusula.Training.HealthCare.Titles
{
    public static class TitleConsts
    {
        private const string DefaultSorting = "{0}Name asc";

        public static string GetDefaultSorting(bool withEntityName) => string.Format(DefaultSorting, withEntityName ? "Title." : string.Empty);

        public const int NameMinLength = 2;
        public const int NameMaxLength = 30;
    }
}
