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
    public class EmployeeController : HealthCareController, IEmployeesAppService
    {
        private readonly IEmployeesAppService _employeesAppService;

        public EmployeeController(IEmployeesAppService employeesAppService)
        {
            _employeesAppService = employeesAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<EmployeeWithNavigationPropertiesDto>> GetListAsync(GetEmployeesInput input) =>
            _employeesAppService.GetListAsync(input);

        [HttpGet]
        [Route("with-navigation-properties/{id}")]
        public virtual Task<EmployeeWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id) =>
            _employeesAppService.GetWithNavigationPropertiesAsync(id);

        [HttpGet]
        [Route("{id}")]
        public virtual Task<EmployeeDto> GetAsync(Guid id) => _employeesAppService.GetAsync(id);

        [HttpPost]
        public virtual Task<EmployeeDto> CreateAsync(EmployeeCreateDto input) => _employeesAppService.CreateAsync(input);

        [HttpPut]
        [Route("{id}")]
        public virtual Task<EmployeeDto> UpdateAsync(EmployeeUpdateDto input) => _employeesAppService.UpdateAsync(input);

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id) => _employeesAppService.DeleteAsync(id);

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(EmployeeExcelDownloadDto input) =>
            _employeesAppService.GetListAsExcelFileAsync(input);

        [HttpGet]
        [Route("download-token")]
        public virtual Task<DownloadTokenResultDto> GetDownloadTokenAsync() =>
            _employeesAppService.GetDownloadTokenAsync();

        [HttpDelete]
        [Route("bulk-delete")]
        public virtual Task DeleteByIdsAsync(List<Guid> doctorIds) => _employeesAppService.DeleteByIdsAsync(doctorIds);

        [HttpDelete]
        [Route("delete-all")]
        public virtual Task DeleteAllAsync(GetEmployeesInput input) => _employeesAppService.DeleteAllAsync(input);
    }
}
