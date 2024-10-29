using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Pusula.Training.HealthCare.Employees;

public class EmployeeViewedEventHandler(ILogger<EmployeeViewedEventHandler> logger)
    : IDistributedEventHandler<EmployeeViewedEto>, ITransientDependency
{
    public Task HandleEventAsync(EmployeeViewedEto eventData)
    {
        logger.LogInformation(
            $" [HANDLER] -> Employee {eventData.Id} viewed as {eventData.ViewedAt.ToLongTimeString()}.");

        return Task.CompletedTask;
    }
}