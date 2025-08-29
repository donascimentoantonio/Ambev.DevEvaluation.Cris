using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// Event dispatcher that logs all domain events using Serilog or Microsoft.Extensions.Logging.
/// </summary>
public class LoggingEventDispatcher : IEventDispatcher
{
    private readonly ILogger<LoggingEventDispatcher> _logger;
    public LoggingEventDispatcher(ILogger<LoggingEventDispatcher> logger)
    {
        _logger = logger;
    }
    public Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Domain event dispatched: {EventType}", domainEvent.GetType().Name);
        return Task.CompletedTask;
    }
}
