namespace Pusula.Training.HealthCare.Titles
{
    public class TitleExcelDownloadDto
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public string? Name { get; set; }

        public TitleExcelDownloadDto()
        {
        }
    }
}
