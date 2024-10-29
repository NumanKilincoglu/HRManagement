using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pusula.Training.HealthCare.Leaves;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Pusula.Training.HealthCare.Handlers;

public class LeaveDeletedEventHandler(ILogger<LeaveDeletedEventHandler> logger)
    : IDistributedEventHandler<LeaveDeletedEto>, ITransientDependency
{
    public Task HandleEventAsync(LeaveDeletedEto eventData)
    {
        logger.LogInformation(
            $" [HANDLER] -> Leave {eventData.Id} deleted at {eventData.DeletedAt.ToLongTimeString()}.");

        return Task.CompletedTask;
    }
}