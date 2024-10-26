using Volo.Abp.Application.Dtos;

namespace Pusula.Training.HealthCare.Titles
{
    public class GetTitlesInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }
        public string? Name { get; set; }
        
        public GetTitlesInput()
        {
        }
    }
}
