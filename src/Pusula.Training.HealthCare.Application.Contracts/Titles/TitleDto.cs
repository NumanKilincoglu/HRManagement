using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Pusula.Training.HealthCare.Titles
{
    public class TitleDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; } = null!;
    }
}
