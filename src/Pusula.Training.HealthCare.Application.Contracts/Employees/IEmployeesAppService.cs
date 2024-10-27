using Pusula.Training.HealthCare.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Pusula.Training.HealthCare.Employees;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Pusula.Training.HealthCare.Employees
{
    public interface IEmployeesAppService : IApplicationService
    {
        Task<PagedResultDto<EmployeeDto>> GetListAsync(GetEmployeesInput input);

        Task<EmployeeWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<EmployeeDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<EmployeeDto> CreateAsync(EmployeeCreateDto input);

        Task<EmployeeDto> UpdateAsync(EmployeeUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(EmployeeExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> doctorIds);

        Task DeleteAllAsync(GetEmployeesInput input);
        Task<Pusula.Training.HealthCare.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}
