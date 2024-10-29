using AutoMapper;
using System;
using Pusula.Training.HealthCare.Employees;
using Pusula.Training.HealthCare.Leaves;
using Pusula.Training.HealthCare.Shared;

namespace Pusula.Training.HealthCare;

public class HealthCareApplicationAutoMapperProfile : Profile
{
    public HealthCareApplicationAutoMapperProfile()
    {
        CreateMap<Employee, EmployeeDto>();
        CreateMap<Employee, EmployeeExcelDto>();
        CreateMap<Employee, EmployeeUpdateDto>();
        CreateMap<EmployeeDto, EmployeeUpdateDto>();
        CreateMap<Employee, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.FirstName));
        CreateMap<EmployeeWithNavigationProperties, EmployeeWithNavigationPropertiesDto>();
        CreateMap<EmployeeDto, EmployeeCacheItem>();
        
        CreateMap<Leave, LeaveDto>();
        CreateMap<Leave, LeaveUpdateDto>();
        CreateMap<LeaveDto, LeaveUpdateDto>();
 
    }
}
