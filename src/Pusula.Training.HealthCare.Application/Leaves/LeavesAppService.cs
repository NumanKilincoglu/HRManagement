using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using MiniExcelLibs;
using Pusula.Training.HealthCare.Employees;
using Pusula.Training.HealthCare.Permissions;
using Pusula.Training.HealthCare.Shared;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Content;
using Volo.Abp.EventBus.Distributed;
using DistributedCacheEntryOptions = Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions;

namespace Pusula.Training.HealthCare.Leaves;

[RemoteService(IsEnabled = false)]
[Authorize(HealthCarePermissions.Leaves.Default)]
public class LeavesAppService(
    ILeaveRepository leaveRepository,
    LeaveManager leaveManager,
    IDistributedCache<LeaveDownloadTokenCacheItem, string> downloadTokenCache,
    IDistributedEventBus distributedEventBus
) : HealthCareAppService, ILeavesAppService
{
    [Authorize(HealthCarePermissions.Leaves.Edit)]
    public async Task<LeaveDto> UpdateAsync(Guid id, LeaveUpdateDto input) =>
        ObjectMapper.Map<Leave, LeaveDto>(
            await leaveManager.UpdateAsync(
                id,
                input.EmployeeId,
                input.StartDate,
                input.EndDate,
                input.LeaveType,
                input.Status)
        );

    public async Task<PagedResultDto<LeaveDto>> GetListAsync(GetLeavesInput input)
    {
        var items = await leaveRepository.GetListAsync(
            input.EmployeeId,
            input.StartDate,
            input.EndDate,
            input.LeaveType,
            input.Status,
            input.Sorting
        );

        var count = await leaveRepository.GetCountAsync(
            input.EmployeeId,
            input.StartDate,
            input.EndDate,
            input.LeaveType,
            input.Status
        );

        return new PagedResultDto<LeaveDto>
        {
            TotalCount = count,
            Items = ObjectMapper.Map<List<Leave>, List<LeaveDto>>(items)
        };
    }

    public async Task<LeaveDto> GetAsync(Guid id)
    {
        await distributedEventBus.PublishAsync(new LeaveViewedEto { Id = id, ViewedAt = Clock.Now },
            onUnitOfWorkComplete: false);
        return ObjectMapper.Map<Leave, LeaveDto>(await leaveRepository.GetAsync(id));
    }

    [Authorize(HealthCarePermissions.Leaves.Create)]
    public async Task<LeaveDto> CreateAsync(LeaveCreateDto input) =>
        ObjectMapper.Map<Leave, LeaveDto>(await leaveManager.CreateAsync(
            input.EmployeeId,
            input.StartDate,
            input.EndDate,
            input.LeaveType,
            input.Status
        ));

    [Authorize(HealthCarePermissions.Leaves.Delete)]
    public async Task DeleteAsync(Guid leaveId)
    {
        await distributedEventBus.PublishAsync(new LeaveDeletedEto { Id = leaveId, DeletedAt = Clock.Now },
            onUnitOfWorkComplete: true);
        await leaveRepository.DeleteAsync(leaveId);
    }

    [Authorize(HealthCarePermissions.Leaves.Delete)]
    public async Task DeleteByIdsAsync(List<Guid> ids) =>
        await leaveRepository.DeleteManyAsync(ids);

    public async Task<IRemoteStreamContent> GetListAsExcelFileAsync(LeaveExcelDownloadDto input)
    {
        var downloadToken = await downloadTokenCache.GetAsync(input.DownloadToken);
        if (downloadToken == null || input.DownloadToken != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
        }

        var leaves = await leaveRepository.GetListAsync(
            input.EmployeeId,
            input.StartDate,
            input.EndDate,
            input.LeaveType,
            input.Status
        );

        var items = leaves.Select(leave => new LeaveExcelDto()
        {
            StartDate = leave.StartDate,
            EndDate = leave.EndDate,
            Status = leave.Status,
            LeaveType = leave.LeaveType,
        });

        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(items);
        memoryStream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(memoryStream, "LeaveList.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");

        await downloadTokenCache.SetAsync(
            token,
            new LeaveDownloadTokenCacheItem { Token = token },
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(200)
            });

        return new Shared.DownloadTokenResultDto
        {
            Token = token
        };
    }

    [Authorize(HealthCarePermissions.Leaves.Delete)]
    public async Task DeleteAllAsync(GetLeavesInput input) =>
        await leaveRepository.DeleteAllAsync(
            input.StartDate,
            input.EndDate,
            input.LeaveType,
            input.Status
        );
}