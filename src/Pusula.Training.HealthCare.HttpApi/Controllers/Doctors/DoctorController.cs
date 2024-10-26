using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Pusula.Training.HealthCare.Doctors;
using Pusula.Training.HealthCare.Doctors;
using Pusula.Training.HealthCare.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace Pusula.Training.HealthCare.Controllers.Doctors
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Doctors")]
    [Route("api/app/doctors")]
    public class DoctorController(IDoctorsAppService doctorsAppService) : HealthCareController, IDoctorsAppService
    {
        [HttpGet]
        public virtual Task<PagedResultDto<DoctorWithNavigationPropertiesDto>> GetListAsync(GetDoctorsInput input) => doctorsAppService.GetListAsync(input);

        [HttpGet]
        [Route("with-navigation-properties/{id}")]
        public virtual Task<DoctorWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id) => doctorsAppService.GetWithNavigationPropertiesAsync(id);

        [HttpGet]
        [Route("{id}")]
        public virtual Task<DoctorDto> GetAsync(Guid id) => doctorsAppService.GetAsync(id);

        [HttpGet]
        [Route("title-lookup")]
        public virtual Task<PagedResultDto<LookupDto<Guid>>> GetTitleLookupAsync(LookupRequestDto input) => doctorsAppService.GetTitleLookupAsync(input);

        [HttpGet]
        [Route("department-lookup")]
        public virtual Task<PagedResultDto<LookupDto<Guid>>> GetDepartmentLookupAsync(LookupRequestDto input) => doctorsAppService.GetDepartmentLookupAsync(input);

        [HttpPost]
        public virtual Task<DoctorDto> CreateAsync(DoctorCreateDto input) => doctorsAppService.CreateAsync(input);

        [HttpPut]
        public virtual Task<DoctorDto> UpdateAsync(DoctorUpdateDto input) => doctorsAppService.UpdateAsync(input);

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id) => doctorsAppService.DeleteAsync(id);

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(DoctorExcelDownloadDto input) => doctorsAppService.GetListAsExcelFileAsync(input);

        [HttpGet]
        [Route("download-token")]
        public virtual Task<DownloadTokenResultDto> GetDownloadTokenAsync() => doctorsAppService.GetDownloadTokenAsync();

        [HttpDelete]
        [Route("")]
        public virtual Task DeleteByIdsAsync(List<Guid> doctorIds) => doctorsAppService.DeleteByIdsAsync(doctorIds);

        [HttpDelete]
        [Route("all")]
        public virtual Task DeleteAllAsync(GetDoctorsInput input) => doctorsAppService.DeleteAllAsync(input);
    }
}
