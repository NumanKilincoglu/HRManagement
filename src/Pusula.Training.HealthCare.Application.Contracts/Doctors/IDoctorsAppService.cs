using Pusula.Training.HealthCare.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Pusula.Training.HealthCare.Doctors
{
    public interface IDoctorsAppService : IApplicationService
    {
        Task<PagedResultDto<DoctorWithNavigationPropertiesDto>> GetListAsync(GetDoctorsInput input);

        Task<DoctorWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<DoctorDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetTitleLookupAsync(LookupRequestDto input);

        Task<PagedResultDto<LookupDto<Guid>>> GetDepartmentLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<DoctorDto> CreateAsync(DoctorCreateDto input);

        Task<DoctorDto> UpdateAsync(DoctorUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(DoctorExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> doctorIds);

        Task DeleteAllAsync(GetDoctorsInput input);
        Task<Pusula.Training.HealthCare.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}
