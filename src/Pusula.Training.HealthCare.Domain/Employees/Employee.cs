using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Pusula.Training.HealthCare.Employees;

public class Employee : FullAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string FirstName { get; set; }

    [NotNull]
    public virtual string LastName { get; set; }

    public virtual DateTime BirthDate { get; set; }

    [NotNull]
    public virtual string IdentityNumber { get; set; }

    [NotNull]
    public virtual string EmailAddress { get; set; }

    [NotNull]
    public virtual string MobilePhoneNumber { get; set; }

    [CanBeNull]
    public virtual string? HomePhoneNumber { get; set; }
    
    [NotNull]
    public virtual double Salary { get; set; }
    
    [CanBeNull]
    public virtual int Gender { get; set; }
    
    protected Employee()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        IdentityNumber = string.Empty;
        EmailAddress = string.Empty;
        MobilePhoneNumber = string.Empty;
        Salary = 0.0;
    }
    
    public Employee(Guid id, string firstName, 
        string lastName, DateTime birthDate,
        string identityNumber, string emailAddress, 
        string mobilePhoneNumber, int gender, 
        string? homePhoneNumber = null)
    {

        Id = id;
        Check.NotNull(firstName, nameof(firstName));
        Check.Length(firstName, nameof(firstName), EmployeeConsts.FirstNameMaxLength, 0);
        Check.NotNull(lastName, nameof(lastName));
        Check.Length(lastName, nameof(lastName), EmployeeConsts.LastNameMaxLength, 0);
        Check.NotNull(identityNumber, nameof(identityNumber));
        Check.Length(identityNumber, nameof(identityNumber), EmployeeConsts.IdentityNumberMaxLength, 0);
        Check.NotNull(emailAddress, nameof(emailAddress));
        Check.Length(emailAddress, nameof(emailAddress), EmployeeConsts.EmailAddressMaxLength, 0);
        Check.NotNull(mobilePhoneNumber, nameof(mobilePhoneNumber));
        Check.Length(mobilePhoneNumber, nameof(mobilePhoneNumber), EmployeeConsts.MobilePhoneNumberMaxLength, 0);
        Check.Range((int)gender, nameof(gender), EmployeeConsts.GenderMinLength, EmployeeConsts.GenderMaxLength);
        
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        IdentityNumber = identityNumber;
        EmailAddress = emailAddress;
        MobilePhoneNumber = mobilePhoneNumber;
        Gender = gender;
        HomePhoneNumber = homePhoneNumber;
        Salary = 0.0;
    }
}