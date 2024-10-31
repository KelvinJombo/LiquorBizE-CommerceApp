namespace Cart.Api.ShoppingCart.GetCart
{
    public record GetCartQuery(string UserName) : IQuery<GetCartResult>;

    public record GetCartResult(ShoppingCarts Cart);

    public class GetCartQueryHandler : IQueryHandler<GetCartQuery, GetCartResult>
    {
        public async Task<GetCartResult> Handle(GetCartQuery query, CancellationToken cancellationToken)
        {
            //Get Cart From Database
            //var cart = await _repository.GetCart(request.UserName);
            return new GetCartResult(new ShoppingCarts("swn"));
        }
    }
}
