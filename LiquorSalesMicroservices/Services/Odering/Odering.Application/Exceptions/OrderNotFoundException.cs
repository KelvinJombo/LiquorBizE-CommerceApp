using BuildingBlocks.Exceptions;

namespace Odering.Application.Exceptions
{
    public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(Guid id) : base("Order", id)
        {
            
        }
    }
}
