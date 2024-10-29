using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pusula.Training.HealthCare.Employees;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Pusula.Training.HealthCare.Handlers;

public class EmployeeDeletedEventHandler(ILogger<EmployeeDeletedEventHandler> logger)
    : IDistributedEventHandler<EmployeeDeletedEto>, ITransientDependency
{
    public Task HandleEventAsync(EmployeeDeletedEto eventData)
    {
        logger.LogInformation(
            $" [HANDLER] -> Employee {eventData.Id} deleted at {eventData.DeletedAt.ToLongTimeString()}.");

        return Task.CompletedTask;
    }
}