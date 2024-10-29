using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Pusula.Training.HealthCare.Leaves;

public interface ILeavesAppService : IApplicationService
{
    Task<LeaveDto> CreateAsync(LeaveCreateDto input);

    Task<LeaveDto> UpdateAsync(Guid id, LeaveUpdateDto input);
    
    Task<PagedResultDto<LeaveDto>> GetListAsync(GetLeavesInput input);
    
    Task<LeaveDto> GetAsync(Guid id);

    Task DeleteAsync(Guid leaveId);
    
    Task DeleteAllAsync(GetLeavesInput input);

    Task DeleteByIdsAsync(List<Guid> ids);

}