using Odering.Domain.Models;

namespace Odering.Domain.Events
{
    public record OrderUpdatedEvent(Order order) : IDomainEvent;
    
    
}
