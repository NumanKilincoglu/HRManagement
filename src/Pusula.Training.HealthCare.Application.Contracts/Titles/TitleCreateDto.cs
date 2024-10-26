using System.ComponentModel.DataAnnotations;

namespace Pusula.Training.HealthCare.Titles
{
    public class TitleCreateDto
    {
        [Required]
        [StringLength(TitleConsts.NameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
