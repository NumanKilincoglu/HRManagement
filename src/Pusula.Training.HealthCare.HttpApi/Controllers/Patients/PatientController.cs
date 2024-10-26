using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Pusula.Training.HealthCare.Patients;
using Pusula.Training.HealthCare.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace Pusula.Training.HealthCare.Controllers.Patients;

[RemoteService]
[Area("app")]
[ControllerName("Patient")]
[Route("api/app/patients")]

public class PatientController(IPatientsAppService patientsAppService) : HealthCareController, IPatientsAppService
{
    [HttpGet]
    public virtual Task<PagedResultDto<PatientDto>> GetListAsync(GetPatientsInput input) => patientsAppService.GetListAsync(input);

    [HttpGet]
    [Route("{id}")]
    public virtual Task<PatientDto> GetAsync(Guid id) => patientsAppService.GetAsync(id);


    [HttpPost]
    public virtual Task<PatientDto> CreateAsync(PatientCreateDto input) => patientsAppService.CreateAsync(input);

    [HttpPut]
    [Route("{id}")]
    public virtual Task<PatientDto> UpdateAsync(Guid id, PatientUpdateDto input) => patientsAppService.UpdateAsync(id, input);

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id) => patientsAppService.DeleteAsync(id);

    [HttpGet]
    [Route("as-excel-file")]
    public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(PatientExcelDownloadDto input) => patientsAppService.GetListAsExcelFileAsync(input);

    [HttpGet]
    [Route("download-token")]
    public virtual Task<DownloadTokenResultDto> GetDownloadTokenAsync() => patientsAppService.GetDownloadTokenAsync();

    [HttpDelete]
    [Route("")]
    public virtual Task DeleteByIdsAsync(List<Guid> patientIds) => patientsAppService.DeleteByIdsAsync(patientIds);

    [HttpDelete]
    [Route("all")]
    public virtual Task DeleteAllAsync(GetPatientsInput input) => patientsAppService.DeleteAllAsync(input);
}