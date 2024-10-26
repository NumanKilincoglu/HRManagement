using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Volo.Abp.Domain.Repositories;

namespace Pusula.Training.HealthCare.Doctors
{
    public interface IDoctorRepository : IRepository<Doctor, Guid>
    {
        Task DeleteAllAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            EnumGender? gender = null,
            Guid? departmentId = null,
            Guid? titleId = null,
            CancellationToken cancellationToken = default);

        Task<DoctorWithNavigationProperties> GetWithNavigationPropertiesAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<List<DoctorWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            EnumGender? gender = null,
            Guid? departmentId = null,
            Guid? titleId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default);


        Task<List<Doctor>> GetListAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            EnumGender? gender = null,
            Guid? departmentId = null,
            Guid? titleId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            EnumGender? gender = null,
            Guid? departmentId = null,
            Guid? titleId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default);
    }
}
