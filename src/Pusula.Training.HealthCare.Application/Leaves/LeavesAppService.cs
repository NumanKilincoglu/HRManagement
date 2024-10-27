using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Pusula.Training.HealthCare.Employees;
using Pusula.Training.HealthCare.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Caching;

namespace Pusula.Training.HealthCare.Leaves;

[RemoteService(IsEnabled = false)]
[Authorize(HealthCarePermissions.Leaves.Default)]

public class LeavesAppService(
    ILeaveRepository leaveRepository,
    LeaveManager leaveManager,
    IDistributedCache<LeavesDownloadTokenCacheItem, string> downloadTokenCache
) : HealthCareAppService, ILeavesAppService
{
    public Task<LeaveDto> UpdateAsync(LeaveUpdateDto input)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResultDto<LeaveDto>> GetListAsync(GetLeavesInput input)
    {
        var items = await leaveRepository.GetListAsync(
            input.EmployeeId,
            input.startDate,
            input.endDate,
            input.LeaveType,
            input.Status
        );

        var count = await leaveRepository.GetCountAsync(
            input.EmployeeId,
            input.startDate,
            input.endDate,
            input.LeaveType,
            input.Status
        );

        return new PagedResultDto<LeaveDto>
        {
            TotalCount = count,
            Items = ObjectMapper.Map<List<Leave>, List<LeaveDto>>(items)
        };
    }

    public Task<LeaveDto> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task CreateAsync(LeaveCreateDto input)
    {
        throw new NotImplementedException();
    }

    Task<LeaveDto> ILeavesAppService.CreateAsync(LeaveCreateDto input)
    {
        throw new NotImplementedException();
    }
    

    public Task<LeaveDto> GetByIdAsync(Guid leaveId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid leaveId)
    {
        return null;
    }
}