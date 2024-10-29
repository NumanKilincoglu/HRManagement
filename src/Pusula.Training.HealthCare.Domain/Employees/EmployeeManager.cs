using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Pusula.Training.HealthCare.Employees;

public class EmployeeManager(IEmployeeRepository employeeRepository) : DomainService
{
    public virtual async Task<Employee> CreateAsync(
        string firstName, string lastName, DateTime birthDate, string identityNumber, string emailAddress,
        string phoneNumber,
        int gender, string? homePhoneNumber = null)
    {
        Check.NotNullOrWhiteSpace(firstName, nameof(firstName), EmployeeConsts.FirstNameMaxLength,
            EmployeeConsts.FirstNameMinLength);
        Check.NotNullOrWhiteSpace(lastName, nameof(lastName), EmployeeConsts.LastNameMaxLength,
            EmployeeConsts.LastNameMinLength);
        Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber), EmployeeConsts.PhoneNumberMaxLength);
        Check.NotNullOrWhiteSpace(emailAddress, nameof(emailAddress));
        Check.Length(emailAddress, nameof(emailAddress), EmployeeConsts.EmailAddressMaxLength, 0);
        Check.NotNull(birthDate, nameof(birthDate));
        Check.Range(gender, nameof(gender), EmployeeConsts.GenderMinLength, EmployeeConsts.GenderMaxLength);

        var employee = new Employee(
            GuidGenerator.Create(), firstName, lastName, birthDate, identityNumber, emailAddress, phoneNumber, gender, homePhoneNumber
        );

        return await employeeRepository.InsertAsync(employee);
    }

    public virtual async Task<Employee> UpdateAsync(
        Guid id, string firstName, string lastName, DateTime birthDate, string identityNumber, string emailAddress,
        string mobilePhoneNumber,
        int gender, double salary, string? homePhoneNumber = null)

    {
        Check.NotNullOrWhiteSpace(firstName, nameof(firstName), EmployeeConsts.FirstNameMaxLength,
            EmployeeConsts.FirstNameMinLength);
        Check.NotNullOrWhiteSpace(lastName, nameof(lastName), EmployeeConsts.LastNameMaxLength,
            EmployeeConsts.LastNameMinLength);
        Check.NotNullOrWhiteSpace(mobilePhoneNumber, nameof(mobilePhoneNumber), EmployeeConsts.PhoneNumberMaxLength);
        Check.NotNullOrWhiteSpace(homePhoneNumber, nameof(homePhoneNumber), EmployeeConsts.PhoneNumberMaxLength);
        Check.NotNullOrWhiteSpace(emailAddress, nameof(emailAddress));
        Check.Length(emailAddress, nameof(emailAddress), EmployeeConsts.EmailAddressMaxLength, 0);
        Check.NotNull(birthDate, nameof(birthDate));
        Check.Range(gender, nameof(gender), EmployeeConsts.GenderMinLength, EmployeeConsts.GenderMaxLength);
        Check.Range(salary, nameof(salary), 0, double.MaxValue);

        var employee = await employeeRepository.GetAsync(id);

        employee.FirstName = firstName;
        employee.LastName = lastName;
        employee.MobilePhoneNumber = mobilePhoneNumber;
        employee.EmailAddress = emailAddress;
        employee.HomePhoneNumber = homePhoneNumber;
        employee.BirthDate = birthDate;
        employee.Gender = gender;
        employee.Salary = salary;

        return await employeeRepository.UpdateAsync(employee);
    }
}