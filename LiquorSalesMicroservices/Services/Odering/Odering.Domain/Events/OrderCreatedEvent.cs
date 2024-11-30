using Odering.Domain.Models;

namespace Odering.Domain.Events
{
    public record OrderCreatedEvent(Order order) : IDomainEvent;
    

    
}
