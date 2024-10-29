using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Pusula.Training.HealthCare.Leaves;
using Pusula.Training.HealthCare.Shared;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace Pusula.Training.HealthCare.Controllers.Leaves
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Leaves")]
    [Route("api/app/leaves")]
    public class LeaveController(ILeavesAppService leavesAppService)
        : HealthCareController, ILeavesAppService
    {
        [HttpGet]
        public virtual Task<PagedResultDto<LeaveDto>> GetListAsync(GetLeavesInput input) =>
            leavesAppService.GetListAsync(input);

        [HttpGet]
        [Route("{id}")]
        public virtual Task<LeaveDto> GetAsync(Guid id) => leavesAppService.GetAsync(id);

        [HttpPost]
        public virtual Task<LeaveDto> CreateAsync(LeaveCreateDto input) => leavesAppService.CreateAsync(input);

        [HttpPut]
        [Route("{id}")]
        public Task<LeaveDto> UpdateAsync(Guid id, LeaveUpdateDto input) => leavesAppService.UpdateAsync(id, input);

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id) => leavesAppService.DeleteAsync(id);

        [HttpDelete]
        [Route("bulk-delete")]
        public Task DeleteByIdsAsync(List<Guid> leaveIds) => leavesAppService.DeleteByIdsAsync(leaveIds);

        [HttpGet]
        [Route("as-excel-file")]
        public Task<IRemoteStreamContent> GetListAsExcelFileAsync(LeaveExcelDownloadDto input) =>
            leavesAppService.GetListAsExcelFileAsync(input);

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync() =>
            leavesAppService.GetDownloadTokenAsync();

        [HttpDelete]
        [Route("delete-all")]
        public virtual Task DeleteAllAsync(GetLeavesInput input) => leavesAppService.DeleteAllAsync(input);
    }
}