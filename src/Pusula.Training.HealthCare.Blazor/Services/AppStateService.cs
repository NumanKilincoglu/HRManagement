using System;
using Pusula.Training.HealthCare.Employees;

namespace Pusula.Training.HealthCare.Blazor.Services;

public class AppStateService
{

    public EmployeeDto? SelectedEmployee { get; private set; }
    
    public event Action OnChange;

    public void SetSelectedEmployee(EmployeeDto employee)
    {
        SelectedEmployee = employee;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}