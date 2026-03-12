using ClaimsEngine.Domain.SeedWork.Abstractions;
using ClaimsEngine.Infra.Data.Outbox;
using Microsoft.EntityFrameworkCore;
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
            if (context == null)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            InsertOutboxMessages(context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            var context = eventData.Context;
            if (context == null)
                return base.SavingChanges(eventData, result);

            InsertOutboxMessages(context);
            return base.SavingChanges(eventData, result);
        }

        private static void InsertOutboxMessages(DbContext? context)
        {
            if (context == null)
                return;

            var aggregates = context.ChangeTracker
                .Entries<AggregateRoot>()
                .Where(a => a.Entity.DomainEvents.Count > 0)
                .Select(a => a.Entity)
                .ToList();

            var events = aggregates
                .SelectMany(a =>
                {
                    var events = a.DomainEvents.ToList();
                    a.ClearDomainEvents();
                    return events;
                })
                .ToList();

            var outboxMessages = events.Select(e => new OutboxMessage()
            {
                Id = Guid.NewGuid(),
                Type = e.GetType().Name ?? string.Empty,
                Content = System.Text.Json.JsonSerializer.Serialize(e),
                CreatedAt = e.OccurredOn
            }).ToList();

            context.Set<OutboxMessage>().AddRange(outboxMessages);
        }
    }
}