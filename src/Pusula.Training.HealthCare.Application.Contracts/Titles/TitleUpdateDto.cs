using System;
using System.ComponentModel.DataAnnotations;

namespace Pusula.Training.HealthCare.Titles
{
    public class TitleUpdateDto
    {
        [Required]
        public Guid Id { get; set; } = default!;

        [Required]
        [StringLength(TitleConsts.NameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
