using Microsoft.EntityFrameworkCore;
using Torneo.Domain.Common;
using Torneo.Infrastructure.Persistence;

namespace Torneo.Infrastructure.Events;

internal sealed class DomainEventDispatcher
{
    private readonly TorneoDbContext _context;
    private readonly DomainEventLog _log;

    public DomainEventDispatcher(TorneoDbContext context, DomainEventLog log)
    {
        _context = context;
        _log = log;
    }

    public void Dispatch()
    {
        foreach (var entry in _context.ChangeTracker.Entries<Entity>().ToArray())
        {
            var events = entry.Entity.DomainEvents.ToArray();
            entry.Entity.ClearDomainEvents();
            foreach (var domainEvent in events)
            {
                _log.Write(domainEvent);
            }
        }
    }

    public void Discard()
    {
        foreach (var entry in _context.ChangeTracker.Entries<Entity>())
        {
            entry.Entity.ClearDomainEvents();
        }
    }
}
