using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pusula.Training.HealthCare.Leaves;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Pusula.Training.HealthCare.Leaves;

public interface ILeavesAppService : IApplicationService
{
    Task<LeaveDto> CreateAsync(LeaveCreateDto input);

    Task<LeaveDto> UpdateAsync(LeaveUpdateDto input);
    
    Task<PagedResultDto<LeaveDto>> GetListAsync(GetLeavesInput input);
    
    Task<LeaveDto> GetAsync(Guid id);

    Task DeleteAsync(Guid leaveId);
    
}