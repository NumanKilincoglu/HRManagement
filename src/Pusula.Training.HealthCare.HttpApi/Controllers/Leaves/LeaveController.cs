using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Pusula.Training.HealthCare.Leaves;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

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

        public Task DeleteByIdsAsync(List<Guid> ids) => leavesAppService.DeleteByIdsAsync(ids);
    }
}
