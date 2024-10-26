using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Pusula.Training.HealthCare.Doctors
{
    public class DoctorManager(IDoctorRepository doctorRepository) : DomainService
    {
        public virtual async Task<Doctor> CreateAsync(
            Guid titleId, Guid departmentId, string firstName, string lastName, string phoneNumber, DateTime birthDate, EnumGender gender)
        {
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName), DoctorConsts.FirstNameMaxLength, DoctorConsts.FirstNameMinLength);
            Check.NotNullOrWhiteSpace(lastName, nameof(lastName), DoctorConsts.LastNameMaxLength, DoctorConsts.LastNameMinLength);
            Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber), DoctorConsts.PhoneNumberMaxLength);
            Check.NotNull(birthDate, nameof(birthDate));
            Check.Range((int)gender, nameof(gender), 1, 2);
            Check.NotNullOrWhiteSpace(departmentId.ToString(), nameof(departmentId));
            Check.NotNullOrWhiteSpace(titleId.ToString(), nameof(titleId));

            var doctor = new Doctor(
                GuidGenerator.Create(), titleId, departmentId, firstName, lastName, phoneNumber, birthDate, gender
                );

            return await doctorRepository.InsertAsync(doctor);
        }

        public virtual async Task<Doctor> UpdateAsync(
            Guid id, Guid titleId, Guid departmentId, string firstName, string lastName, string phoneNumber, DateTime birthDate, EnumGender gender)

        {
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName), DoctorConsts.FirstNameMaxLength, DoctorConsts.FirstNameMinLength);
            Check.NotNullOrWhiteSpace(lastName, nameof(lastName), DoctorConsts.LastNameMaxLength, DoctorConsts.LastNameMinLength);
            Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber), DoctorConsts.PhoneNumberMaxLength);
            Check.NotNull(birthDate, nameof(birthDate));
            Check.Range((int)gender, nameof(gender), 1, 2);
            Check.NotNullOrWhiteSpace(departmentId.ToString(), nameof(departmentId));
            Check.NotNullOrWhiteSpace(titleId.ToString(), nameof(titleId));

            var doctor = await doctorRepository.GetAsync(id);

            doctor.TitleId = titleId;
            doctor.DepartmentId = departmentId;
            doctor.FirstName = firstName;
            doctor.LastName = lastName;
            doctor.PhoneNumber = phoneNumber;
            doctor.BirthDate = birthDate;
            doctor.Gender = gender;

            return await doctorRepository.UpdateAsync(doctor);
        }

    }
}
