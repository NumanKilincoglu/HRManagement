using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Pusula.Training.HealthCare.Employees;
using Pusula.Training.HealthCare.Shared;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace Pusula.Training.HealthCare.Controllers.Employees
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Employees")]
    [Route("api/app/employees")]
    public class EmployeeController(IEmployeesAppService employeesAppService)
        : HealthCareController, IEmployeesAppService
    {
        [HttpGet]
        public virtual Task<PagedResultDto<EmployeeDto>> GetListAsync(GetEmployeesInput input) =>
            employeesAppService.GetListAsync(input);

        [HttpGet]
        [Route("with-navigation-properties/{id}")]
        public virtual Task<EmployeeWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id) =>
            employeesAppService.GetWithNavigationPropertiesAsync(id);

        [HttpGet]
        [Route("{id}")]
        public virtual Task<EmployeeDto> GetAsync(Guid id) => employeesAppService.GetAsync(id);

        [HttpPost]
        public virtual Task<EmployeeDto> CreateAsync(EmployeeCreateDto input) => employeesAppService.CreateAsync(input);

        [HttpPut]
        [Route("{id}")]
        public virtual Task<EmployeeDto> UpdateAsync(EmployeeUpdateDto input) => employeesAppService.UpdateAsync(input);

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id) => employeesAppService.DeleteAsync(id);

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(EmployeeExcelDownloadDto input) =>
            employeesAppService.GetListAsExcelFileAsync(input);

        [HttpGet]
        [Route("download-token")]
        public virtual Task<DownloadTokenResultDto> GetDownloadTokenAsync() =>
            employeesAppService.GetDownloadTokenAsync();

        [HttpDelete]
        [Route("bulk-delete")]
        public virtual Task DeleteByIdsAsync(List<Guid> employeeIds) => employeesAppService.DeleteByIdsAsync(employeeIds);

        [HttpDelete]
        [Route("delete-all")]
        public virtual Task DeleteAllAsync(GetEmployeesInput input) => employeesAppService.DeleteAllAsync(input);
    }
}
