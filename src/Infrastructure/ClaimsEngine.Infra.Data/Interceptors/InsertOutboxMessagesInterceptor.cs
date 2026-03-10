using ClaimsEngine.Domain.SeedWork;
using ClaimsEngine.Infra.Data.Outbox;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ClaimsEngine.Infra.Data.Interceptors
{
    public sealed class InsertOutboxMessagesInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, 
            InterceptionResult<int> result, 
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if(context == null) 
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            // 1. Find all AggregateRoots with domain events
            var aggregates = context.ChangeTracker
                .Entries<AggregateRoot>()
                .Where(a => a.Entity.DomainEvents.Any())
                .Select(a => a.Entity)
                .ToList();

            // 2. Extract and serialize the events
            var events = aggregates
                .SelectMany(a =>
                {
                    var events = a.DomainEvents.ToList();
                    a.ClearDomainEvents(); // Clear events after extracting
                    return events;
                })
                .ToList();

            // 3. Create OutboxMessage entities and add to context
            var outboxMessages = events.Select(e =>
            {
                return new OutboxMessage()
                {
                    Id = Guid.NewGuid(),
                    Type = e.GetType().Name ?? string.Empty,
                    Content = System.Text.Json.JsonSerializer.Serialize(e),
                    CreatedAt = e.OccurredOn
                };
            }).ToList();

            // 4. Add to the DbContext in the same transaction
            context.Set<OutboxMessage>().AddRange(outboxMessages);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}