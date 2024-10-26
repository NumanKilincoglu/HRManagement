using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Pusula.Training.HealthCare.Doctors
{
    public class Doctor : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string FirstName { get; set; }
        [NotNull]
        public virtual string LastName { get; set; }
        [NotNull]
        public virtual string PhoneNumber { get; set; }
        [NotNull]
        public virtual DateTime BirthDate { get; set; }
        [CanBeNull]
        public virtual EnumGender Gender { get; set; }

        public virtual Guid DepartmentId { get; set; }
        public virtual Guid TitleId { get; set; }

        protected Doctor()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            BirthDate = DateTime.Now;
        }

        public Doctor(Guid id, Guid titleId, Guid departmentId, string firstName, string lastName, string phoneNumber, DateTime birthDate, EnumGender gender = default)
        {
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName), DoctorConsts.FirstNameMaxLength, DoctorConsts.FirstNameMinLength);
            Check.NotNullOrWhiteSpace(lastName, nameof(lastName), DoctorConsts.LastNameMaxLength, DoctorConsts.LastNameMinLength);
            Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber), DoctorConsts.PhoneNumberMaxLength);
            Check.NotNull(birthDate, nameof(birthDate));
            Check.Range((int)gender, nameof(gender), 1, 2);
            Check.NotNullOrWhiteSpace(departmentId.ToString(), nameof(departmentId));
            Check.NotNullOrWhiteSpace(titleId.ToString(), nameof(titleId));


            Id = id;
            TitleId = titleId;
            DepartmentId = departmentId;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
            Gender = gender;
        }
    }
}
