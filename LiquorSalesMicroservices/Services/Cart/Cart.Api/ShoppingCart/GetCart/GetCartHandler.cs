using Cart.Api.Data;

namespace Cart.Api.ShoppingCart.GetCart
{
    public record GetCartQuery(string UserName) : IQuery<GetCartResult>;

    public record GetCartResult(ShoppingCarts Cart);

    public class GetCartQueryHandler(ICartRepository repository) : IQueryHandler<GetCartQuery, GetCartResult>
    {
        public async Task<GetCartResult> Handle(GetCartQuery query, CancellationToken cancellationToken)
        {
            
            var cart = await repository.GetCart(query.UserName);
            return new GetCartResult(cart);
        }
    }
}
